using Lagrande.History.HistoryItem;
using Lagrande.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lagrande.History
{
    public class HistoryViewModel: NotifiableObject
    {
        private List<HistoryItemViewModel> history = new List<HistoryItemViewModel>();
        public List<HistoryItemViewModel> History
        {
            get => history;
            set
            {
                if (history == value)
                    return;
                history = value;
                OnPropertyChanged();
                HistoryChanged?.Invoke(value.Select(item => item.Model).ToList());
            }
        }

        public ICommand ClearCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand DoubleClickCommand { get; }

        public event Action<List<ProblemModel>> HistoryChanged;
        public event Action<ProblemModel> ModelSelected;

        public HistoryViewModel()
        {
            ClearCommand = new CallbackCommand(Clear);
            RemoveItemCommand = new CallbackCommand(RemoveItem);
            DoubleClickCommand = new CallbackCommand(SelectItem);
        }

        public void ApplyModel(List<ProblemModel> models)
        {
            History = models.Select(item => new HistoryItemViewModel(item)).ToList();
        }

        public void Add(ProblemModel model)
        {
            var newList = new List<HistoryItemViewModel>(History);
            newList.Add(new HistoryItemViewModel(model));
            History = newList;
        }

        public void Clear(object param)
        {
            History = new List<HistoryItemViewModel>();
        }

        public void SelectItem(object param)
        {
            if (!(param is HistoryItemViewModel))
                return;
            ModelSelected?.Invoke((param as HistoryItemViewModel).Model);
        }

        public void RemoveItem(object param)
        {
            if (param is HistoryItemViewModel)
            {
                var newList = new List<HistoryItemViewModel>(History);
                newList.Remove(param as HistoryItemViewModel);
                History = newList;
            }
        }
    }
}
