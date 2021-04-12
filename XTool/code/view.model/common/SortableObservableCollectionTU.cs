using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;

namespace XTool
{
    public class SortableObservableCollection<T, U> : ObservableCollection<U> where U : SortableViewModel<T>, new()
    {

        public Func<T> GenerateModel { get; set; }
        private ICommand _AddItemCommand;
        private List<T> _Items;

        public ICommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new RelayCommand(
                        param => AddItem());
                }
                return _AddItemCommand;
            }
        }

        public SortableObservableCollection()
        {
            _Items = new List<T>();
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        public SortableObservableCollection(List<T> models)
        {
            _Items = models;
            int count = models.Count;
            for (int i = 0; i < count; i++)
            {
                U u = new U() { };
                u.Initialize(models[i]);
                u.ItemIndex = i;
                u.ItemsCount = count;
                u.MoveUp = MoveUp;
                u.MoveDown = MoveDown;
                u.Remove = Remove;
                Add(u);
            }
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    SortableViewModel<T> vm = item as SortableViewModel<T>;
                    vm.MoveUp = MoveUp;
                    vm.MoveDown = MoveDown;
                    vm.Remove = Remove;
                    _Items.Add(vm.Model);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    SortableViewModel<T> vm = item as SortableViewModel<T>;
                    _Items.Remove(vm.Model);
                    
                }
            }
            ReIndex();
        }

        private void ReIndex()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].ItemIndex = i;
                this[i].ItemsCount = this.Count;
            }
        }


        public void Remove(object viewModel)
        {
            if (viewModel is U)
            {
                U vm = viewModel as U;
                int i = IndexOf(vm);
                RemoveAt(i);
                int j = _Items.IndexOf(vm.Model);
                if (j > -1)
                {
                    _Items.RemoveAt(j);
                }                
            }
        }

        public void MoveUp(object viewModel)
        {
            if (viewModel is U)
            {
                U vm = viewModel as U;
                int x = IndexOf(vm);
                int y = x - 1;
                vm.ItemIndex = y;
                this[y].ItemIndex = x;
                Move(x, y);
                Swap(x, y);
            }
        }

        public void MoveDown(object viewModel)
        {
            if (viewModel is U)
            {
                U vm = viewModel as U;
                int x = IndexOf(vm);
                int y = x + 1;
                vm.ItemIndex = y;
                this[y].ItemIndex = x;
                Move(x, y);
                Swap(x, y);
            }
        }

        private void Swap(int x, int y)
        {
            List<int> k = new List<int>() { x, y };
            List<T> temp = new List<T>();
            for (int i = 0; i < _Items.Count; i++)
            {
                if (i == k[0])
                {
                    temp.Add(_Items[k[1]]);
                }
                else if (i == k[1])
                {
                    temp.Add(_Items[k[0]]);
                }
                else
                {
                    temp.Add(_Items[i]);
                }
            }
            _Items.Clear();
            _Items.AddRange(temp);

        }

        private void AddItem()
        {
            T t = default(T);
            if (GenerateModel != null)
            {
                t = GenerateModel();
            }
            U u = new U() { Model = t, MoveUp = MoveUp, MoveDown = MoveDown, Remove = Remove };
            Add(u);
        }
    }
}
