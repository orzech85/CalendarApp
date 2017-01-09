
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CalendarApp.Droid
{
    public class CalendarView : ViewGroup
    {
        public CalendarView(Context context) :
            base(context)
        {
            Initialize();
        }

        public CalendarView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public CalendarView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            CreateCalendar();
        }

        void BtnPrevious_Click(object sender, EventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            CreateCalendar();
        }

        void BtnNext_Click(object sender, EventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            CreateCalendar();
        }

        Button btnPrevious;
        Button btnNext;
        TextView lblCurrentMonth;
        List<TextView> daysOfWeek;
        List<Button> days;

        DayOfWeek firstDayOfWeek => CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek;

        void CreateCalendar()
        {
            RemoveAllViews();

            if (CurrentMonth == DateTime.MinValue)
            {
                CurrentMonth = DateTime.Now;
            }

            CreateTitle();
            CreateDaysOfWeek();
            CreateDays();
        }

        void CreateTitle()
        {
            btnPrevious = new Button(Context);
            btnPrevious.Text = "<";
            btnPrevious.Click += BtnPrevious_Click;

            lblCurrentMonth = new TextView(Context);
			lblCurrentMonth.Text = CurrentMonth.ToString("MMMMM yyyy");
            lblCurrentMonth.SetBackgroundColor(Color.Blue);

            btnNext = new Button(Context);
            btnNext.Text = ">";
            btnNext.Click += BtnNext_Click;

            AddView(btnPrevious);
            AddView(lblCurrentMonth);
            AddView(btnNext);
        }

        void CreateDaysOfWeek()
        {
            daysOfWeek = new List<TextView>();

            string[] names = CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedDayNames;
            string saturdayName = names[(int)DayOfWeek.Saturday];
            string sundayName = names[(int)DayOfWeek.Sunday];

            foreach (var name in names.Shift((int)firstDayOfWeek))
            {
                TextView dayOfWeek = new TextView(Context);
                dayOfWeek.Text = name;

                if ((name == saturdayName) || (name == sundayName))
                    dayOfWeek.SetTextColor(Color.Red);

                daysOfWeek.Add(dayOfWeek);
                AddView(dayOfWeek);
            }
        }

        void CreateDays()
        {
            days = new List<Button>();

            DateTime dayOfMonth = CurrentMonth;

            for (int i = 1; i <= DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month); i++)
            {
                Button day = new Button(Context);
                day.Text = i.ToString();

                if ((dayOfMonth >= FromDate)
                    && (dayOfMonth <= ToDate))
                    day.SetBackgroundColor(Color.Blue);
                else
                    day.SetBackgroundColor(Color.Transparent);

                if ((dayOfMonth.DayOfWeek == DayOfWeek.Saturday) || (dayOfMonth.DayOfWeek == DayOfWeek.Sunday))
                {
                    day.SetTextColor(Color.Red);
                }

                days.Add(day);
                AddView(day);

                dayOfMonth = dayOfMonth.AddDays(1);
            }
        }

        private DateTime _currentMonth;        
        public DateTime CurrentMonth { 
            get { return _currentMonth; }
            set { _currentMonth = new DateTime(value.Year, value.Month, 1); }
        }

        public DateTime SelectDate {
            set
            {
                FromDate = value;
                ToDate = value;
            }
        }

        private DateTime _fromDate;
        public DateTime FromDate { 
            get { return _fromDate; }
            set { _fromDate = value; CreateCalendar(); }
        }

        private DateTime _toDate;
        public DateTime ToDate { 
            get { return _toDate; }
            set { _toDate = value; CreateCalendar(); }
        }

        int rowHeight;
        int colWidth;

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            rowHeight = (bottom - top) / 8;
            colWidth = (right - left) / 7;

            LayoutTitle(changed, left, top, right, bottom);
            LayoutDayOfWeeks(changed, left, top, right, bottom);
            LayoutDays(changed, left, top, right, bottom);
        }

        void LayoutTitle(bool changed, int left, int top, int right, int bottom)
        {
            btnPrevious.Layout(
                0,
                0,
                rowHeight,
                rowHeight
            );

            lblCurrentMonth.Layout(
                rowHeight,
                0,
                right - left - rowHeight,
                rowHeight
            );

            btnNext.Layout(
                right - left - rowHeight,
                0,
                right - left,
                rowHeight
            );
        }

        void LayoutDayOfWeeks(bool changed, int left, int top, int right, int bottom)
        {
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek[i].Layout(
                    colWidth * i,
                    rowHeight,
                    colWidth * i + colWidth,
                    rowHeight * 2
                );
            }
        }

        void LayoutDays(bool changed, int left, int top, int right, int bottom)
        {
            DateTime currentDay = CurrentMonth;
            int currentRow = 2;

            int currentCol = ((int)currentDay.DayOfWeek) - ((int)firstDayOfWeek);

            if (currentCol < 0)
                currentCol = currentCol + 7;

            for (int i = 0; i < DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month); i++)
            {
                days[i].Layout(
                    colWidth * currentCol,
                    rowHeight * currentRow,
                    colWidth * (currentCol + 1),
                    rowHeight * (currentRow + 1)
                );

                currentCol++;

                if (currentCol > 6)
                {
					currentCol = 0;
                    currentRow++;
                }
            }
        }
   }
}
