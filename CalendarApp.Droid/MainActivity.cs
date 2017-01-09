using Android.App;
using Android.Widget;
using Android.OS;

namespace CalendarApp.Droid
{
    [Activity(Label = "CalendarApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            DateRangePickerView rangePicker = FindViewById<DateRangePickerView>(Resource.Id.range_picker);
            CalendarView calendar = FindViewById<CalendarView>(Resource.Id.calendar);

            calendar.FromDate = rangePicker.FromDate;
            calendar.ToDate = rangePicker.ToDate;

            rangePicker.OnDateRangeChanged += (fromDate, toDate) => 
            { 
                calendar.FromDate = fromDate;
                calendar.ToDate = toDate;
            };
        }
    }
}

