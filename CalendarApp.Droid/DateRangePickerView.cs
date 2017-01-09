
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
    public class DateRangePickerView : LinearLayout
    {
        public DateRangePickerView(Context context) :
            base(context)
        {
            Initialize();
        }

        public DateRangePickerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public DateRangePickerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;

            FromDatePicker = new DatePickerView(Context) { Text = "From:" };
            ToDatePicker = new DatePickerView(Context) { Text = "To:" };

            if ((FromDate == DateTime.MinValue) || (ToDate == DateTime.MinValue))
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            
            SetDatesLimits();

            FromDatePicker.OnDateChanged += OnDateChanged;
			ToDatePicker.OnDateChanged += OnDateChanged;
			
            AddView(FromDatePicker);
            AddView(ToDatePicker);
        }

        void OnDateChanged(DateTime obj)
        {
            SetDatesLimits();
            OnDateRangeChanged?.Invoke(FromDatePicker.SelectedDate, ToDatePicker.SelectedDate);
        }

        void SetDatesLimits()
        {
            FromDatePicker.MinDate = DateTime.MinValue;
            FromDatePicker.MaxDate = ToDatePicker.SelectedDate;
            ToDatePicker.MinDate = FromDatePicker.SelectedDate;
            ToDatePicker.MaxDate = DateTime.MaxValue;
        }

        public event Action<DateTime, DateTime> OnDateRangeChanged;

        public DateTime FromDate
        {
            get { return FromDatePicker.SelectedDate; }
            set { FromDatePicker.SelectedDate = value; }
        }

        public DateTime ToDate
        {
            get { return ToDatePicker.SelectedDate; }
            set { ToDatePicker.SelectedDate = value; }
        }

        DatePickerView FromDatePicker;
        DatePickerView ToDatePicker;
    }
}
