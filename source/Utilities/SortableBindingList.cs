using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ACEhole
{
    /// <summary>
    /// BindingList with DataGridView column sorting support.
    /// Handles Memory column sort via MemoryBytes for ModInfo.
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool              _isSorted;
        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        public SortableBindingList()            : base() { }
        public SortableBindingList(IList<T> l) : base(l) { }

        protected override bool              SupportsSortingCore  => true;
        protected override bool              IsSortedCore         => _isSorted;
        protected override PropertyDescriptor SortPropertyCore    => _sortProperty;
        protected override ListSortDirection  SortDirectionCore   => _sortDirection;
        protected override bool              SupportsSearchingCore => true;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var items = (List<T>)this.Items;

            if (prop.PropertyType.GetInterface("IComparable") != null)
            {
                items.Sort((x, y) =>
                {
                    object xv = prop.GetValue(x);
                    object yv = prop.GetValue(y);

                    // Sort Memory column by MemoryBytes so "20 KB" sorts correctly against "1 MB"
                    if (prop.Name == "Memory" && typeof(T).Name == "ModInfo")
                    {
                        var mb = TypeDescriptor.GetProperties(typeof(T))["MemoryBytes"];
                        if (mb != null) { xv = mb.GetValue(x); yv = mb.GetValue(y); }
                    }

                    if (xv == null && yv == null) return 0;
                    if (xv == null) return direction == ListSortDirection.Ascending ? -1 : 1;
                    if (yv == null) return direction == ListSortDirection.Ascending ?  1 : -1;

                    if (xv is string xs && yv is string ys)
                    {
                        if (string.IsNullOrEmpty(xs) && string.IsNullOrEmpty(ys)) return 0;
                        if (string.IsNullOrEmpty(xs)) return direction == ListSortDirection.Ascending ? -1 : 1;
                        if (string.IsNullOrEmpty(ys)) return direction == ListSortDirection.Ascending ?  1 : -1;
                    }

                    int r = ((IComparable)xv).CompareTo(yv);
                    return direction == ListSortDirection.Ascending ? r : -r;
                });

                _isSorted = true; _sortProperty = prop; _sortDirection = direction;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        protected override void RemoveSortCore()
        {
            _isSorted = false; _sortProperty = null;
            _sortDirection = ListSortDirection.Ascending;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }
}
