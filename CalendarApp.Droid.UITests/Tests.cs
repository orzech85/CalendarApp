using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace CalendarApp.Droid.UITests
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {            
            app = ConfigureApp.Android.StartApp();
            //app.Repl();

            NowDay = DateTime.Now.Day;
            NowMonth = DateTime.Now.Month;
            NowYear = DateTime.Now.Year;
        }

        int NowDay;
        int NowMonth;
        int NowYear;

        [Test]
        public void InitialState_TodayDate()
        {
            app.WaitForElement(c => c.Text("From:"));

            long yesterday = 0;
            long today = 0;
            long tomorrow = 0;

            if (NowDay > 1)
            {
                yesterday = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay-1).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();
            }

			today = (long)app.Query(c => c.Descendant("CalendarView").Button().Text(NowDay.ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();

            if (NowDay < DateTime.DaysInMonth(NowYear, NowMonth))
            {
                tomorrow = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay - 1).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();
            }

			Assert.AreEqual(0, yesterday);
            Assert.AreEqual(-16776961, today);
            Assert.AreEqual(0, tomorrow);
        }

        [Test]
        public void ChangeFromDate_MinusTwoDays()
        {
            app.WaitForElement(c => c.Text("From:"));

            var fromDayMinusButton = app.Query(c => c.Text("-"))[0];

            for (int i = 0; i < 2; i++)
            {
                app.TapCoordinates(fromDayMinusButton.Rect.CenterX, fromDayMinusButton.Rect.CenterY);
            }

            long threeDaysBefore = 0;
            long twoDaysBefore = 0;
            long oneDayBefore = 0;
            long today = 0;
            long tomorrow = 0;


            if (NowDay>3)
                threeDaysBefore = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay - 3).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();
            if (NowDay>2)
                twoDaysBefore = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay - 2).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();
            if (NowDay>1)
                oneDayBefore = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay - 1).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();
            
			today = (long)app.Query(c => c.Descendant("CalendarView").Button().Text(NowDay.ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();

            if (NowDay<DateTime.DaysInMonth(NowYear, NowMonth))
                tomorrow = (long)app.Query(c => c.Descendant("CalendarView").Button().Text((NowDay + 1).ToString()).Invoke("getBackground").Invoke("getColor")).SingleOrDefault();

            Assert.AreEqual(0, threeDaysBefore);
            Assert.AreEqual(-16776961, twoDaysBefore);
            Assert.AreEqual(-16776961, oneDayBefore);
            Assert.AreEqual(-16776961, today);
            Assert.AreEqual(0, tomorrow);
        }
    }
}
