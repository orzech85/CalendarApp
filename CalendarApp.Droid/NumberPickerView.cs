
using System;
using System.Collections.Generic;
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
    public class NumberPickerView : LinearLayout
    {
        public NumberPickerView(Context context) :
            base(context)
        {
            Initialize();
        }

        public NumberPickerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public NumberPickerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            SetGravity(GravityFlags.Center);
            Orientation = Orientation.Vertical;

            btnPlus = new Button(Context);
            btnPlus.Text = "+";
            btnPlus.Click += BtnPlus_Click;

			txtValue = new TextView(Context);
			txtValue.Text = CurrentValue.ToString();
            txtValue.Gravity = GravityFlags.Center;
            txtValue.TextAlignment = TextAlignment.Gravity;
			
            btnMinus = new Button(Context);
            btnMinus.Text = "-";
            btnMinus.Click += BtnMinus_Click;

            AddView(btnPlus);
            AddView(txtValue);
			AddView(btnMinus);
        }

        void BtnMinus_Click(object sender, EventArgs e)
        {
            if (CurrentValue > MinValue)
            {
                CurrentValue--;
                txtValue.Text = CurrentValue.ToString();
                OnNumberChanged?.Invoke(CurrentValue);
            }
        }

        void BtnPlus_Click(object sender, EventArgs e)
        {
            if (CurrentValue < MaxValue)
            {
                CurrentValue++;
                txtValue.Text = CurrentValue.ToString();
                OnNumberChanged?.Invoke(CurrentValue);
            }
        }

        public event Action<int> OnNumberChanged;

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        private int _currentValue;

        public int CurrentValue { 
            get { return _currentValue; }
            set { 
                _currentValue = value; 
                txtValue.Text = CurrentValue.ToString(); 
            }
        }

        Button btnPlus;
        Button btnMinus;
        TextView txtValue;
    }
}
