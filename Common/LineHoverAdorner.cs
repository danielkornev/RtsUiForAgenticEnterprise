using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace MutliModalLiveCopilotWpfApp.Common
{
    public class LineHoverAdorner : Adorner
    {
        internal ContentPresenter Container { get; set; }

        public LineHoverAdorner(UIElement adornedElement) : base(adornedElement) => Container = new ContentPresenter();

        protected override Size MeasureOverride(Size constraint)
        {
            Container.Measure(constraint);
            return Container.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var line = (System.Windows.Shapes.Line)AdornedElement;
            Container.Arrange(new Rect(new Point(line.X1, line.Y1), line.DesiredSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => Container;

        protected override int VisualChildrenCount => 1;
    } // class
} // namespace
