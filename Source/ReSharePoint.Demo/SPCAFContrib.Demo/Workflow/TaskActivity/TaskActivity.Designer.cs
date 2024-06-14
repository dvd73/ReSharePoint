using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace SPCAFContrib.Demo.Workflow.TaskActivity
{
    public partial class TaskActivity
    {
        #region Activity Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        [System.CodeDom.Compiler.GeneratedCode("", "")]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            this.deleteCancelledTask = new Microsoft.SharePoint.WorkflowActions.DeleteTask();
            this.isNotNeedDeleteCancelledTask = new System.Workflow.Activities.IfElseBranchActivity();
            this.isNeedDeleteCancelledTask = new System.Workflow.Activities.IfElseBranchActivity();
            this.deleteTask = new Microsoft.SharePoint.WorkflowActions.DeleteTask();
            this.ifNeedDeleteCancelledTask = new System.Workflow.Activities.IfElseActivity();
            this.setCancelled = new System.Workflow.Activities.CodeActivity();
            this.isNotNeedDeleteTask = new System.Workflow.Activities.IfElseBranchActivity();
            this.isNeedDeleteTask = new System.Workflow.Activities.IfElseBranchActivity();
            this.onTaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.cancellationHandler = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.ifNeedDeleteTask = new System.Workflow.Activities.IfElseActivity();
            this.completeTask = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.whileNotCompleted = new System.Workflow.Activities.WhileActivity();
            this.createTask = new Microsoft.SharePoint.WorkflowActions.CreateTask();
            // 
            // deleteCancelledTask
            // 
            correlationtoken1.Name = "taskToken";
            correlationtoken1.OwnerActivityName = "TaskActivity";
            this.deleteCancelledTask.CorrelationToken = correlationtoken1;
            this.deleteCancelledTask.Name = "deleteCancelledTask";
            activitybind1.Name = "TaskActivity";
            activitybind1.Path = "TaskGuid";
            this.deleteCancelledTask.MethodInvoking += new System.EventHandler(this.deleteTask_MethodInvoking);
            this.deleteCancelledTask.SetBinding(Microsoft.SharePoint.WorkflowActions.DeleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // isNotNeedDeleteCancelledTask
            // 
            this.isNotNeedDeleteCancelledTask.Name = "isNotNeedDeleteCancelledTask";
            // 
            // isNeedDeleteCancelledTask
            // 
            this.isNeedDeleteCancelledTask.Activities.Add(this.deleteCancelledTask);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.needDelete);
            this.isNeedDeleteCancelledTask.Condition = codecondition1;
            this.isNeedDeleteCancelledTask.Name = "isNeedDeleteCancelledTask";
            // 
            // deleteTask
            // 
            this.deleteTask.CorrelationToken = correlationtoken1;
            this.deleteTask.Name = "deleteTask";
            activitybind2.Name = "TaskActivity";
            activitybind2.Path = "TaskGuid";
            this.deleteTask.MethodInvoking += new System.EventHandler(this.deleteTask_MethodInvoking);
            this.deleteTask.SetBinding(Microsoft.SharePoint.WorkflowActions.DeleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // ifNeedDeleteCancelledTask
            // 
            this.ifNeedDeleteCancelledTask.Activities.Add(this.isNeedDeleteCancelledTask);
            this.ifNeedDeleteCancelledTask.Activities.Add(this.isNotNeedDeleteCancelledTask);
            this.ifNeedDeleteCancelledTask.Name = "ifNeedDeleteCancelledTask";
            // 
            // setCancelled
            // 
            this.setCancelled.Name = "setCancelled";
            this.setCancelled.ExecuteCode += new System.EventHandler(this.setCancelled_ExecuteCode);
            // 
            // isNotNeedDeleteTask
            // 
            this.isNotNeedDeleteTask.Name = "isNotNeedDeleteTask";
            // 
            // isNeedDeleteTask
            // 
            this.isNeedDeleteTask.Activities.Add(this.deleteTask);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.needDelete);
            this.isNeedDeleteTask.Condition = codecondition2;
            this.isNeedDeleteTask.Name = "isNeedDeleteTask";
            // 
            // onTaskChanged
            // 
            activitybind3.Name = "TaskActivity";
            activitybind3.Path = "AfterProperties";
            this.onTaskChanged.BeforeProperties = null;
            this.onTaskChanged.CorrelationToken = correlationtoken1;
            activitybind4.Name = "TaskActivity";
            activitybind4.Path = "Executor";
            this.onTaskChanged.Name = "onTaskChanged";
            activitybind5.Name = "TaskActivity";
            activitybind5.Path = "TaskGuid";
            this.onTaskChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onTaskChanged_Invoked);
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.ExecutorProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // cancellationHandler
            // 
            this.cancellationHandler.Activities.Add(this.setCancelled);
            this.cancellationHandler.Activities.Add(this.ifNeedDeleteCancelledTask);
            this.cancellationHandler.Name = "cancellationHandler";
            // 
            // ifNeedDeleteTask
            // 
            this.ifNeedDeleteTask.Activities.Add(this.isNeedDeleteTask);
            this.ifNeedDeleteTask.Activities.Add(this.isNotNeedDeleteTask);
            this.ifNeedDeleteTask.Name = "ifNeedDeleteTask";
            // 
            // completeTask
            // 
            this.completeTask.CorrelationToken = correlationtoken1;
            this.completeTask.Name = "completeTask";
            activitybind6.Name = "TaskActivity";
            activitybind6.Path = "TaskGuid";
            activitybind7.Name = "TaskActivity";
            activitybind7.Path = "TaskResult";
            this.completeTask.MethodInvoking += new System.EventHandler(this.completeTask_MethodInvoking);
            this.completeTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.completeTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // whileNotCompleted
            // 
            this.whileNotCompleted.Activities.Add(this.onTaskChanged);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.notComplete);
            this.whileNotCompleted.Condition = codecondition3;
            this.whileNotCompleted.Name = "whileNotCompleted";
            // 
            // createTask
            // 
            this.createTask.CorrelationToken = correlationtoken1;
            activitybind8.Name = "TaskActivity";
            activitybind8.Path = "TaskId";
            this.createTask.Name = "createTask";
            this.createTask.SpecialPermissions = null;
            activitybind9.Name = "TaskActivity";
            activitybind9.Path = "TaskGuid";
            activitybind10.Name = "TaskActivity";
            activitybind10.Path = "CreateProperties";
            this.createTask.MethodInvoking += new System.EventHandler(this.createTask_MethodInvoking);
            this.createTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.ListItemIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.createTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.createTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // TaskActivity
            // 
            this.Activities.Add(this.createTask);
            this.Activities.Add(this.whileNotCompleted);
            this.Activities.Add(this.completeTask);
            this.Activities.Add(this.ifNeedDeleteTask);
            this.Activities.Add(this.cancellationHandler);
            this.Name = "TaskActivity";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged onTaskChanged;

        private WhileActivity whileNotCompleted;

        private Microsoft.SharePoint.WorkflowActions.CompleteTask completeTask;

        private Microsoft.SharePoint.WorkflowActions.DeleteTask deleteTask;

        private IfElseBranchActivity isNotNeedDeleteTask;

        private IfElseBranchActivity isNeedDeleteTask;

        private IfElseActivity ifNeedDeleteTask;

        private Microsoft.SharePoint.WorkflowActions.DeleteTask deleteCancelledTask;

        private IfElseBranchActivity isNotNeedDeleteCancelledTask;

        private IfElseBranchActivity isNeedDeleteCancelledTask;

        private IfElseActivity ifNeedDeleteCancelledTask;

        private CodeActivity setCancelled;

        private CancellationHandlerActivity cancellationHandler;

        private Microsoft.SharePoint.WorkflowActions.CreateTask createTask;

















    }
}
