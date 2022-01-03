using Osls.SfcEditor;
using Osls.SfcSimulation.Viewer;


namespace Osls.Plants.MinimalExample
{
    /// <summary>
    /// Minimal example class for a test viewer.
    /// </summary>
    public class MinimalTestExample : TestPage
    {
        #region ==================== Fields / Properties ====================
        private bool _isExecutable;
        private IDiagramSimulationMaster _simulationMaster;
        private MinimalSimulationExample _simulation;
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole twat viewer. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            _simulation = GetNode<MinimalSimulationExample>("MinimalSimulationExample");
            string filepath = openedLesson.TemporaryDiagramFilePath;
            SfcEntity sfcEntity = SfcEntity.TryLoadFromFile(filepath);
            if (sfcEntity != null)
            {
                _simulation.InitialiseWith(mainNode, openedLesson);
                _simulationMaster = new Master(sfcEntity, _simulation);
                _isExecutable = _simulationMaster.IsProgramSimulationValid();
            }
            else
            {
                _isExecutable = false;
            }
        }
        
        public override void _Process(float delta)
        {
            int simulationSteps = LookupTargetSimulationCycles();
            for (int i = 0; i < simulationSteps; i++)
            {
                if (_isExecutable)
                {
                    _simulationMaster.UpdateSimulation(16);
                }
            }
        }
        #endregion
    }
}
