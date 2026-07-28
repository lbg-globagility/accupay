using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

namespace SergeUtils
{
    public delegate void CheckBoxClickedHandler(bool state);

    public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
    {
        public bool _isChecked;

        public DataGridViewCheckBoxHeaderCellEventArgs(bool bChecked)
        {
            _isChecked = bChecked;
        }

        public bool Checked
        {
            get { return _isChecked; }
        }
    }

    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private Point checkBoxLocation;

        private Size checkBoxSize;

        private bool _isChecked = false;

        private Point _cellLocation = new Point();

        private CheckBoxState _checkBoxState = CheckBoxState.UncheckedNormal;

        private readonly string _headerText;

        public event CheckBoxClickedHandler OnCheckBoxClicked;

        public DatagridViewCheckBoxHeaderCell(string headerText)
        {
            _headerText = headerText;
        }

        public bool IsChecked => _isChecked;

        public CheckBoxState CheckBoxState => _checkBoxState;

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            if (!string.IsNullOrEmpty(base.OwningColumn?.HeaderText) && base.OwningColumn?.HeaderText != _headerText) base.OwningColumn.HeaderText = _headerText;

            base.Paint(graphics,
                clipBounds,
                cellBounds,
                rowIndex,
                dataGridViewElementState,
                value,
                formattedValue,
                errorText,
                cellStyle,
                advancedBorderStyle,
                paintParts);

            var p = new Point();

            var s = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.CheckedNormal);

            //p.X = cellBounds.Location.X + (cellBounds.Width / 2) - (s.Width / 2);
            p.X = cellBounds.Location.X + (cellBounds.Width - (s.Width + 8));
            p.Y = cellBounds.Location.Y + (cellBounds.Height / 2) - (s.Height / 2);

            _cellLocation = cellBounds.Location;

            checkBoxLocation = p;

            checkBoxSize = s;

            if (_isChecked)
                _checkBoxState = CheckBoxState.CheckedNormal;
            else
                _checkBoxState = CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox(graphics, checkBoxLocation, _checkBoxState);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
            if (p.X >= checkBoxLocation.X && p.X <=
                checkBoxLocation.X + checkBoxSize.Width
            && p.Y >= checkBoxLocation.Y && p.Y <=
                checkBoxLocation.Y + checkBoxSize.Height)
            {
                _isChecked = !_isChecked;
                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(_isChecked);
                    this.DataGridView.InvalidateCell(this);
                }

            }

            base.OnMouseClick(e);
        }

        public void Unset()
        {
            _isChecked = false;
            _checkBoxState = CheckBoxState.UncheckedNormal;
        }
    }
}
