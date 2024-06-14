using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WorkflowActions;
using SharePoint.Common.Utilities.Extensions;

namespace SPCAFContrib.Demo.Workflow.ExecuteStoredProcedure
{
    public partial class ExecuteStoredProcedure : Activity
    {
        #region Properties
        private IDictionary _dictParameters = new Hashtable();
        private IDictionary ConnectionParameters
        {
            get
            {
                return _dictParameters;
            }
            set { _dictParameters = value; }
        }

        public static DependencyProperty __ContextProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__Context", typeof(WorkflowContext), typeof(ExecuteStoredProcedure));
        [Description("Context")]
        [ValidationOption(ValidationOption.Required)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WorkflowContext __Context
        {
            get { return ((WorkflowContext)(base.GetValue(ExecuteStoredProcedure.__ContextProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.__ContextProperty, value); }
        }

        public static DependencyProperty __ListIdProperty = System.Workflow.ComponentModel.DependencyProperty.Register("__ListId", typeof(string), typeof(ExecuteStoredProcedure));
        [Description("Id of the list")]
        [Category("String Transformation")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Required)]
        public string __ListId
        {
            get
            {
                return ((string)(base.GetValue(ExecuteStoredProcedure.__ListIdProperty)));
            }
            set
            {
                base.SetValue(ExecuteStoredProcedure.__ListIdProperty, value);
            }
        }

        public static DependencyProperty __ListItemProperty = DependencyProperty.Register("__ListItem", typeof(int), typeof(ExecuteStoredProcedure));
        [Description("__ListItem")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Required)]
        public int __ListItem
        {
            get { return ((int)(base.GetValue(ExecuteStoredProcedure.__ListItemProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.__ListItemProperty, value); }
        }

        public static DependencyProperty ConnectionStringProperty = System.Workflow.ComponentModel.DependencyProperty.Register("ConnectionString", typeof(string), typeof(ExecuteStoredProcedure));
        [Description("Specifies the connection string")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Required)]
        public string ConnectionString
        {
            get { return ((string)(base.GetValue(ExecuteStoredProcedure.ConnectionStringProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.ConnectionStringProperty, value); }
        }

        public static DependencyProperty SPNameProperty = System.Workflow.ComponentModel.DependencyProperty.Register("SPName", typeof(string), typeof(ExecuteStoredProcedure));
        [Description("SPName")]
        [ValidationOption(ValidationOption.Required)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SPName
        {
            get { return ((string)(base.GetValue(ExecuteStoredProcedure.SPNameProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.SPNameProperty, value); }
        }

        public static DependencyProperty SPParamsProperty = System.Workflow.ComponentModel.DependencyProperty.Register("SPParams", typeof(string), typeof(ExecuteStoredProcedure));
        [Description("SPParams")]
        [Category("This is the category which will be displayed in the Property Browser")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [ValidationOption(ValidationOption.Optional)]
        public string SPParams
        {
            get { return ((string)(base.GetValue(ExecuteStoredProcedure.SPParamsProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.SPParamsProperty, value); }
        }

        public static DependencyProperty VariableProperty = System.Workflow.ComponentModel.DependencyProperty.Register("Variable", typeof(string), typeof(ExecuteStoredProcedure));
        [Description("Variable")]
        [Category("Variable")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Variable
        {
            get { return ((string)(base.GetValue(ExecuteStoredProcedure.VariableProperty))); }
            set { base.SetValue(ExecuteStoredProcedure.VariableProperty, value); }
        }

        private SPListItem _spListItem;
        protected SPListItem spListItem
        {
            get { return _spListItem; }
            set { _spListItem = value; }
        }

        private SPFieldType _spFieldType;
        protected SPFieldType spFieldType
        {
            get { return _spFieldType; }
            set { _spFieldType = value; }
        }
        #endregion
        #region Constructors

        public ExecuteStoredProcedure()
        {
            InitializeComponent();
        }
        #endregion
        #region Methods

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {

            using (WorkflowHistoryTraceListener myTracer = new WorkflowHistoryTraceListener(executionContext, this.WorkflowInstanceId))
            {
                Trace.Listeners.Add(myTracer);
                try
                {
                    using (SPSite sourceSite = new SPSite(this.__Context.Web.Site.ID))
                    {
                        using (SPWeb sourceWeb = sourceSite.AllWebs[this.__Context.Web.ID])
                        {
                            SPList spList = __Context.Web.Lists[new Guid(this.__ListId)];
                            this.spListItem = spList.GetItemById(this.__ListItem);

                            ArrayList alParaMeterItems = SPParameterName(this.SPParams);
                            ArrayList alSPListItems = SelectListItemsValues(this.spListItem, alParaMeterItems);
                            Data data = new Data(this.ConnectionString);
                            this.Variable = Convert.ToString(data.ExecuteStoredProcedure(this.SPName, alSPListItems));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorWriting(ex);
                    ex.LogError();
                    Common.LogExceptionToWorkflowHistory("Error occurred when Execute Store Procedure workflow executing.", executionContext, this.WorkflowInstanceId);
                    throw ex;
                }
                Trace.Listeners.Remove(myTracer);
            }
            return base.Execute(executionContext);
        }
        /// <summary>
        /// Error message variable.
        /// </summary>
        /// <param name="errorMessage"></param>
        void ErrorWriting(Exception errorMessage)
        {
            if (errorMessage.ToString().ToLower().Contains("connection"))
            {
                this.Variable = "-5";
            }
            else if (errorMessage.ToString().ToLower().Contains("stored procedure"))
            {
                this.Variable = "-6";
            }
            else if (errorMessage.ToString().ToLower().Contains("parameter") || errorMessage.ToString().ToLower().Contains("arguments"))
            {
                this.Variable = "-7";
            }
            else
            {
                this.Variable = "-8";
            }
        }

        /// <summary>
        /// Return parameters.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected ArrayList SPParameterName(string parameterName)
        {
            ArrayList alSourceItems = new ArrayList();

            if (parameterName != null)
            {
                string[] strValue = parameterName.Split(',');
                alSourceItems.AddRange(strValue);
            }
            return alSourceItems;
        }

        /// <summary>
        /// Return the splist item values.
        /// </summary>
        /// <param name="spListItems"></param>
        /// <param name="paraMeters"></param>
        /// <returns></returns>
        protected ArrayList SelectListItemsValues(SPListItem spItem, ArrayList paraMeters)
        {
            ArrayList alSourceItems = new ArrayList();
            
            if (paraMeters.Count > 0)
            {
                for (int con = 0; con < paraMeters.Count; con++)
                {
                    if (paraMeters[con].ToString().Contains("["))
                    {

                        string ConstValue = paraMeters[con].ToString().Replace('[', ' ');
                        ConstValue = ConstValue.Trim().Replace(']', ' ');
                        if (spItem.Fields.ContainsField(ConstValue.Trim()))
                        {
                            if (spItem[ConstValue.Trim()] != null)
                            {
                                alSourceItems.Add(spItem[ConstValue.Trim()].ToString());
                            }
                            else
                            {
                                alSourceItems.Add(string.Empty);
                            }
                        }
                    }
                    else
                    {
                        alSourceItems.Add(paraMeters[con].ToString());
                    }
                }
            }
            
            return alSourceItems;
        }

        protected ArrayList RemoveBraces(ArrayList alParaList)
        {
            ArrayList alRemovedList = new ArrayList();
            for (int con = 0; con < alParaList.Count; con++)
            {
                if (alParaList[con].ToString().Contains("["))
                {
                    string strValue = alParaList[con].ToString().Replace('[', ' ');
                    strValue = strValue.Trim().Replace(']', ' ');
                    alRemovedList.Add(strValue.Trim());
                }
                else
                {
                    alRemovedList.Add(alParaList[con].ToString());
                }
            }
            return alRemovedList;
        }
        #endregion
    }
}
