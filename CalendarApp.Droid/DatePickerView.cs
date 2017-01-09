
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CalendarApp.Droid
{
    public class DatePickerView : LinearLayout
    {
        public DatePickerView(Context context) :
            base(context)
        {
            Initialize();
        }

        public DatePickerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public DatePickerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            if (SelectedDate == DateTime.MinValue)
                SelectedDate = DateTime.Now;

            label = new TextView(Context);

            dayPicker = new NumberPickerView(Context);
            dayPicker.CurrentValue = SelectedDate.Day;
            dayPicker.OnNumberChanged += DayPicker_OnNumberChanged;

            monthPicker = new NumberPickerView(Context);
            monthPicker.CurrentValue = SelectedDate.Month;
            monthPicker.OnNumberChanged += MonthPicker_OnNumberChanged;
            yearPicker = new NumberPickerView(Context);
            yearPicker.CurrentValue = SelectedDate.Year;
            yearPicker.OnNumberChanged += YearPicker_OnNumberChanged;

            AddView(label);
            AddView(dayPicker);
            AddView(monthPicker);
            AddView(yearPicker);
        }

        void DayPicker_OnNumberChanged(int day)
        {
            SelectedDate = new DateTime(SelectedDate.Year, SelectedDate.Month, day);

            SetLimits();

            OnDateChanged?.Invoke(SelectedDate);
        }

        void MonthPicker_OnNumberChanged(int month)
        {
            int day = SelectedDate.Day;
            int daysInMonth = DateTime.DaysInMonth(SelectedDate.Year, month);

            if (daysInMonth < day)
            {
                day = daysInMonth;
                dayPicker.CurrentValue = day;
            }

            SelectedDate = new DateTime(SelectedDate.Year, month, day);

            SetLimits();

            OnDateChanged?.Invoke(SelectedDate);
        }

        void YearPicker_OnNumberChanged(int year)
        {
            int day = SelectedDate.Day;
            int daysInMonth = DateTime.DaysInMonth(year, SelectedDate.Month);

            if (daysInMonth < day)
            {
                day = daysInMonth;
                dayPicker.CurrentValue = day;
            }

            SelectedDate = new DateTime(year, SelectedDate.Month, day);

            SetLimits();

            OnDateChanged?.Invoke(SelectedDate);
        }

        void SetLimits()
        {
            yearPicker.MinValue = MinDate.Year > DateTime.MinValue.Year ? (((SelectedDate.Month < MinDate.Month) || ((SelectedDate.Day < MinDate.Day) && (SelectedDate.Month <= MinDate.Month))) ? MinDate.Year + 1 : MinDate.Year) : DateTime.MinValue.Year;
            yearPicker.MaxValue = MaxDate.Year < DateTime.MaxValue.Year ? (((SelectedDate.Month > MaxDate.Month) || ((SelectedDate.Day > MaxDate.Day) && (SelectedDate.Month >= MaxDate.Month))) ? MaxDate.Year - 1 : MaxDate.Year) : DateTime.MaxValue.Year;

            monthPicker.MinValue = SelectedDate.Year == MinDate.Year ? (SelectedDate.Day < MinDate.Day ? MinDate.Month + 1 : MinDate.Month) : 1;
            monthPicker.MaxValue = SelectedDate.Year == MaxDate.Year ? (SelectedDate.Day > MaxDate.Day ? MaxDate.Month - 1 : MaxDate.Month) : 12;

            dayPicker.MinValue = ((SelectedDate.Year == MinDate.Year) && (SelectedDate.Month == MinDate.Month)) ? MinDate.Day : 1;
            dayPicker.MaxValue = ((SelectedDate.Year == MaxDate.Year) && (SelectedDate.Month == MaxDate.Month)) ? MaxDate.Day : DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month);
        }

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        TextView label;

        NumberPickerView dayPicker;
        NumberPickerView monthPicker;
        NumberPickerView yearPicker;

        public event Action<DateTime> OnDateChanged;

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = new DateTime(value.Year, value.Month, value.Day); }
        }

        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value; SetLimits(); }
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set { _maxDate = value; SetLimits(); }
        }
    }
}
