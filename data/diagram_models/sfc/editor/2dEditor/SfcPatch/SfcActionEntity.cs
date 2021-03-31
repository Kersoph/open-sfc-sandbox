namespace SfcSandbox.Data.Model.SfcEditor
{
    /// <summary>
    /// Entity class for the data in one action description in a patch field
    /// </summary>
    public class SfcActionEntity
    {
        #region ==================== Fields / Properties ====================
        /// <summary>
        /// The Qualifier of the action. (Execution time and setting)
        /// </summary>
        public ActionQualifier Qualifier { get; set; }
        /// <summary>
        /// The action to be executed. (Can be invalid)
        /// </summary>
        public string Action { get; set; }
        #endregion
        
        
        #region ==================== Persistence ====================
        /// <summary>
        /// Loads the data from the stream. Written in "WriteTo".
        /// </summary>
        public static SfcActionEntity CreateFrom(System.IO.BinaryReader reader)
        {
            SfcActionEntity entity = new SfcActionEntity();
            entity.Qualifier = (ActionQualifier)reader.ReadInt32();
            entity.Action = reader.ReadString();
            return entity;
        }
        
        /// <summary>
        /// Writes the data from the stream. Read by "ReadFrom".
        /// </summary>
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write((int)Qualifier);
            writer.Write(Action);
        }
        #endregion
    }
    
    /// <summary>
    /// The way an action should be executed.
    /// </summary>
    public enum ActionQualifier
    {
        N,
        PPlus,
        PMinus
    }
}