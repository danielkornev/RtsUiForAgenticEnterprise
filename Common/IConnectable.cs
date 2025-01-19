using MutliModalLiveCopilotWpfApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutliModalLiveCopilotWpfApp.Common
{
    public interface IConnectable
    {
        ICollection<Drawable> Connections { get; }

        bool IsParent { get; }

        Drawable Parent { get; set; }

        void Move(double offsetX, double offsetY);
    } // interface
} // namespace
