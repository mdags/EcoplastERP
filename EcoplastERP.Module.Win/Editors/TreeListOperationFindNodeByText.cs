using System;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes.Operations;

namespace EcoplastERP.Module.Win.Editors
{
    internal class TreeListOperationFindNodeByText : TreeListOperation, IDisposable
    {
        private TreeListNode foundNode;
        private TreeListColumn column;
        private readonly string substr;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (column != null)
                {
                    column.Dispose();
                    column = null;
                }
        }
        ~TreeListOperationFindNodeByText()
        {
            Dispose(false);
        }
        public TreeListOperationFindNodeByText(TreeListColumn column, string substr)
        {
            foundNode = null;
            this.column = column;
            this.substr = substr;
        }
        public override void Execute(TreeListNode node)
        {
            string s = node[column].ToString();
            if (s.StartsWith(substr))
                foundNode = node;
        }
        public override bool NeedsVisitChildren(TreeListNode node) { return foundNode == null; }
        public TreeListNode Node { get { return foundNode; } }
    }
}
