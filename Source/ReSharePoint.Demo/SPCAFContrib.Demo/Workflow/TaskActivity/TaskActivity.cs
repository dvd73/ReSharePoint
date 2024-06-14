using System;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.Workflow.TaskActivity
{
    /// <summary>
    /// Делегат для callback-вызова метода родительской активности в CompleteTask-активности
    /// </summary>
    /// <param name="taskActivity"></param>
    /// <param name="task"></param>
    public delegate void TaskCallback(TaskActivity taskActivity);

    /// <summary>
    /// Структура для обмена данными между задачей и родительской активностью
    /// </summary>
    [Serializable]
    public struct TaskData
    {
        /// <summary>
        /// Порядковый номер задачи
        /// </summary>
        public int TaskNo;

        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public String Title;

        /// <summary>
        /// Кому назначено
        /// </summary>
        public String AssignedTo;

        /// <summary>
        /// Если true - исполненные задания удаляются
        /// </summary>
        public bool DeleteCompletedTask;

        /// <summary>
        /// Ф-я, которую вызовут по окончании задачи
        /// </summary>
        public TaskCallback OnCompleted;
    }

    public partial class TaskActivity : SequenceActivity
    {

        /// <summary>
        /// Хранище внешних данных
        /// </summary>
        public TaskData Data;

        /// <summary>
        /// Результат работы
        /// </summary>
        public string TaskResult { get; private set; }

        /// <summary>
        /// Фактический исполнитель
        /// </summary>
        public string Executor { get; private set; }

        /// <summary>
        /// Признак того, что обработка задачи была прервана
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// ID задачи в списке (по нему будем искать задачу в callback-функциях)
        /// </summary>
        public int TaskId = -1;

        // Внутренние переменные
        public Guid TaskGuid = default(System.Guid);
        public SPWorkflowTaskProperties CreateProperties = new SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties AfterProperties = new SPWorkflowTaskProperties();

        public TaskActivity()
        {
            InitializeComponent();
        }

        private void createTask_MethodInvoking(object sender, EventArgs e)
        {
            TaskGuid = Guid.NewGuid();

            CreateProperties.Title = Data.Title;
            CreateProperties.AssignedTo = Data.AssignedTo;

            TaskResult = String.Empty;
            Executor = String.Empty;
            IsCancelled = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
            });
        }

        private void notComplete(object sender, ConditionalEventArgs e)
        {
            e.Result = String.IsNullOrEmpty(TaskResult);
        }

        private void needDelete(object sender, ConditionalEventArgs e)
        {
            e.Result = Data.DeleteCompletedTask;
        }

        private void onTaskChanged_Invoked(object sender, ExternalDataEventArgs e)
        {
            // Ждем статус Completed
            if (!(AfterProperties == null) &&
                AfterProperties.ExtendedProperties.ContainsKey(SPBuiltInFieldId.TaskStatus) &&
                "Completed".Equals(AfterProperties.ExtendedProperties[SPBuiltInFieldId.TaskStatus] as String)
                )
                TaskResult = "Completed";
        }

        private void setCancelled_ExecuteCode(object sender, EventArgs e)
        {
            IsCancelled = true;
        }

        private void completeTask_MethodInvoking(object sender, EventArgs e)
        {
            // Вызываем делегата
            if (Data.OnCompleted != null)
                Data.OnCompleted(this);
        }

        private void deleteTask_MethodInvoking(object sender, EventArgs e)
        {
            // Очищаем ID задачи
            TaskId = -1;
        }

    }
}
