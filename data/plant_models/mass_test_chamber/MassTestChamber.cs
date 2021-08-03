using System.Collections.Generic;


namespace Osls.Plants.MassTestChamber
{
    /// <summary>
    /// Boundary class between the simulation and the SFC Controllers
    /// </summary>
    public class MassTestChamber : SimulationPage
    {
        #region ==================== Fields / Properties ====================
        public const string LaserKey = "=C2-E1";
        public const string TemperatureSensorKey = "=C2-B6";
        public const string FieldGeneratorKey = "=A0-G1";
        public const string MassSufficientKey = "=A0-B3";
        public const string DetectorKey = "=A0-B4";
        
        public const string EmitterMotorKey = "=C0-M1";
        public const string EmitterKey = "=C0-H1";
        public const string EmitterBackKey = "=C0-B0";
        public const string EmitterFrontKey = "=C0-B1";
        
        public const string FocusMotorKey = "=C1-M1";
        public const string FocusKey = "=C1-U1";
        public const string FocusBackKey = "=C1-B0";
        public const string FocusFrontKey = "=C1-B1";
        
        public bool FieldGeneratorInput { get; private set; }
        public bool LaserInput { get; private set; }
        public bool EmitterInput { get; private set; }
        public bool FocusInput { get; private set; }
        public int EmitterMotorInput { get; private set; }
        public int FocusMotorInput { get; private set; }
        
        /// <summary>
        /// Links the test chamber
        /// </summary>
        public Chamber Chamber { get { return GetNode<Chamber>("Chamber"); } }
        #endregion
        
        
        #region ==================== Public Methods ====================
        /// <summary>
        /// Initializes the whole page. Called before the node is added to the tree by the lesson controller.
        /// </summary>
        public override void InitialiseWith(IMainNode mainNode, ILessonEntity openedLesson)
        {
            Chamber.Setup();
        }
        
        /// <summary>
        /// Called when the user can have options to influence the simulation.
        /// Normally called by the by the simulation UI
        /// </summary>
        public override void SetupUi()
        {
        }
        #endregion
        
        
        #region ==================== Helpers ====================
        /// <summary>
        /// Returns the input definition for the simulation
        /// </summary>
        protected override StateTable DefineInputs()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(FieldGeneratorKey, false, "Field generator", "True will activate the field generator.\nFalse will turn it off.") },
                    { new StateEntry<bool>(LaserKey, false, "Heating laser", "True will turn on the laser heater.\nFalse will turn it off.") },
                    { new StateEntry<bool>(EmitterKey, false, "Emitter", "True will turn on the particle emitter.\nFalse will turn it off.") },
                    { new StateEntry<bool>(FocusKey, false, "Focus", "True will keep objects in the intended position.\nFalse will turn it off.") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(EmitterMotorKey, 0, "Emitter cart motor", "A signal of 1 will move the chart \ntowards the center and -1 away from it.\nA signal of 0 will keep it in position.") },
                    { new StateEntry<int>(FocusMotorKey, 0, "Focus cart motor", "A signal of 1 will move the chart \ntowards the center and -1 away from it.\nA signal of 0 will keep it in position.") },
                }
            );
        }
        
        /// <summary>
        /// Returns the output definition for the simulation
        /// </summary>
        protected override StateTable DefineOutput()
        {
            return new StateTable(
                new List<StateEntry<bool>>()
                {
                    { new StateEntry<bool>(EmitterBackKey, false, "Emitter cart position: Back", "True if the chart is at the distant position.\nFalse otherwise.") },
                    { new StateEntry<bool>(EmitterFrontKey, false, "Emitter cart position: Front", "True if the chart is at the front position.\nFalse otherwise.") },
                    { new StateEntry<bool>(FocusBackKey, false, "Focus cart position: Back", "True if the chart is at the distant position.\nFalse otherwise.") },
                    { new StateEntry<bool>(FocusFrontKey, false, "Focus cart position: Front", "True if the chart is at the front position.\nFalse otherwise.") },
                    { new StateEntry<bool>(MassSufficientKey, false, "Mass OK", "True if the trapped mass is sufficient.\nFalse otherwise.") },
                    { new StateEntry<bool>(DetectorKey, false, "Discharge detector", "True if the detector receives a signal.\nFalse otherwise.") },
                },
                new List<StateEntry<int>>()
                {
                    { new StateEntry<int>(TemperatureSensorKey, 293, "Central temperature K", "Measured temperature of the central mass.") },
                }
            );
        }
        
        /// <summary>
        /// Calculates the next simulation step.
        /// It will read the SimulationInput values and stores in the end the new values to the SimulationOutput.
        /// </summary>
        protected override void CalculateNextStep(int deltaTime)
        {
            UpdateInputs();
            Chamber.Update(this, deltaTime);
        }
        
        private void UpdateInputs()
        {
            FieldGeneratorInput = SimulationInput.PollBoolean(FieldGeneratorKey);
            LaserInput = SimulationInput.PollBoolean(LaserKey);
            EmitterInput = SimulationInput.PollBoolean(EmitterKey);
            FocusInput = SimulationInput.PollBoolean(FocusKey);
            EmitterMotorInput = SimulationInput.PollInteger(EmitterMotorKey);
            FocusMotorInput = SimulationInput.PollInteger(FocusMotorKey);
        }
        #endregion
    }
}
