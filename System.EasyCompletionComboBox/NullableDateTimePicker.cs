using System;
using System.Windows.Forms;

namespace SergeUtils
{
    public partial class NullableDateTimePicker : UserControl
    {
        private bool _lastCheckValue;

        public event EventHandler CheckedChanged;

        public NullableDateTimePicker()
        {
            InitializeComponent();
        }

        protected virtual void OnCheckedChanged()
        {
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void NullableDateTimePicker_Load(object sender, System.EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.MinValue;
            _lastCheckValue = dateTimePicker1.Checked;
        }

        public new void ResetText()
        {
            dateTimePicker1.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void dateTimePicker1_ValueChanged(object sender, System.EventArgs e)
        {
            if (_lastCheckValue != dateTimePicker1.Checked && !dateTimePicker1.Checked)
            {
                this.Value = null;
            }

            _lastCheckValue = dateTimePicker1.Checked;

            OnCheckedChanged();
        }

        public new DateTime? Value
        {
            get
            {
                if (dateTimePicker1.Checked)
                {
                    return dateTimePicker1.Value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null || value.Value < dateTimePicker1.MinDate)
                {
                    dateTimePicker1.Checked = false;
                }
                else
                {
                    dateTimePicker1.Checked = true;
                    dateTimePicker1.Value = value.Value;
                }
            }
        }

        public override string ToString()
        {
            return this.Value == null
                ? "Nothing"
                : this.Value.Value.ToString("MM/dd/yyyy hh:mm tt");
        }

        public bool Checked
        {
            get
            {
                return dateTimePicker1.Checked;
            }
            set
            {
                dateTimePicker1.Checked = value;
                // if (_lastCheckValue != value) OnCheckedChanged();
            }
        }
    }
}
