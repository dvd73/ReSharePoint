namespace ReSharePoint.Pro.Generators
{
    /*
    [GenerateProvider]
    public class GenerateDisposeItemProvider : IGenerateActionProvider
    {
        public IEnumerable<IGenerateActionWorkflow> CreateWorkflow(IDataContext dataContext)
        {
            var solution = dataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
            var iconManager = solution.GetComponent<PsiIconManager>();
            var icon = iconManager.GetImage(CLRDeclaredElementType.METHOD);
            yield return new GenerateDisposeActionWorkflow(icon);
        }
    }

    public class GenerateDisposeActionWorkflow : StandardGenerateActionWorkflow
    {
        // Using named parameters for clarification
        public GenerateDisposeActionWorkflow(IconId icon)
            : base(kind: "Dispose", icon: icon, menuText: "&VDispose",
                   actionGroup: GenerateActionGroup.CLR_LANGUAGE,
                   windowTitle: "Generate dispose",
                   description: "Generate a Dispose() implementation which disposes selected fields.",
                   actionId: "Generate.Dispose")
        {
        }

        public override double Order
        {
            get { return 100; }
        }
        public override bool IsAvailable(IDataContext dataContext)
        {
            var solution = dataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
            if (solution == null)
                return false;

            var generatorManager = GeneratorManager.GetInstance(solution);
            if (generatorManager == null)
                return false;

            var languageType = generatorManager.GetPsiLanguageFromContext(dataContext);
            if (languageType == null)
                return false;

            var generatorContextFactory = LanguageManager.Instance.TryGetService<IGeneratorContextFactory>(languageType);
            return generatorContextFactory != null;
        }


    }
     */
}
