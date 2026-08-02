using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjectHierarchyTool.XResources
{
    public class DataGridViewButtonCellEx : DataGridViewButtonCell
    {
        public Action? OnClickEvent { get; set; } = null;

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);
            OnClickEvent?.Invoke();
        }
    }
}
