using System.Runtime.InteropServices;

namespace GameObjectHierarchyTool.TreeViewEx
{
    internal class TreeViewEx : TreeView
    {
        public TreeViewEx()
        { }

        #region This extra to reduce the flickering

        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        private const int TVS_EX_DOUBLEBUFFER = 0x4;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x203 && CheckBoxes)
            {
                int x = m.LParam.ToInt32() & 0xffff;
                int y = (m.LParam.ToInt32() >> 16) & 0xffff;
                TreeViewHitTestInfo hitTestInfo = HitTest(x, y);

                if (hitTestInfo.Node != null && hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                {
                    OnBeforeCheck(new TreeViewCancelEventArgs(hitTestInfo.Node, false, TreeViewAction.ByMouse));
                    hitTestInfo.Node.Checked = !hitTestInfo.Node.Checked;
                    OnAfterCheck(new TreeViewEventArgs(hitTestInfo.Node, TreeViewAction.ByMouse));
                    m.Result = IntPtr.Zero;
                    return;
                }
            }

            base.WndProc(ref m);
        }
    }
}
