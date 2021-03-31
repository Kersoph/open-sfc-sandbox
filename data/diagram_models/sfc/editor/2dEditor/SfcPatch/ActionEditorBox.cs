using Godot;
using System.Collections.Generic;

namespace SfcSandbox.Data.Model.SfcEditor
{
    public class ActionEditorBox : Control
    {
        #region ==================== Properties ====================
        private const string _editorScenePath = "res://Data/Model/SfcEditor/2dEditor/SfcPatch/Elements/ActionEditor.tscn";
        
        private SfcPatchControl _patchController;
        private readonly List<ActionEditor> _actionEditors;
        #endregion
        
        
        #region ==================== Public ====================
        public ActionEditorBox()
        {
            _actionEditors = new List<ActionEditor>();
        }
        
        public override void _Ready()
        {
            _patchController = GetNode<SfcPatchNode>("..").SfcPatchControl;
        }
        
        /// <summary>
        /// Called when the model has changed or is initialized.
        /// </summary>
        public void UpdateActions(SfcPatchEntity entity)
        {
            UpdateVisibility(entity);
            for (int i = 0; i < entity.ActionEntries.Count; i++)
            {
                if (i >= _actionEditors.Count) AddActionEditor();
                _actionEditors[i].UpdateAction(entity.ActionEntries[i]);
            }
            
            if (entity.ActionEntries.Count >= _actionEditors.Count) AddActionEditor();
            _actionEditors[entity.ActionEntries.Count].ResetAction();
            
            for (int i = entity.ActionEntries.Count + 1; i < _actionEditors.Count; i++)
            {
                RemoveActionEditor(_actionEditors[i]);
            }
        }
        
        /// <summary>
        /// Called by the action editor to update an entry.
        /// </summary>
        public void UserChangedData(ActionEditor editor, ActionQualifier qualifier, string action)
        {
            int index = _actionEditors.IndexOf(editor);
            _patchController.UpdateActionTo(index, qualifier, action);
        }
        
        /// <summary>
        /// Called by the action editor to add a new entry as it is getting relevant.
        /// </summary>
        public void AddNewAction()
        {
            _patchController.AddNewAction();
        }
        
        /// <summary>
        /// Called by the action editor to remove an entry as it is no longer relevant.
        /// </summary>
        public void RemoveAction(ActionEditor editor)
        {
            int index = _actionEditors.IndexOf(editor);
            _patchController.RemoveActionAt(index);
        }
        #endregion
        
        
        #region ==================== Private Methods ====================
        private void UpdateVisibility(SfcPatchEntity entity)
        {
            this.Visible = entity.ContainsRealStep();
        }
        
        private void AddActionEditor()
        {
            Node node = ((PackedScene)GD.Load(_editorScenePath)).Instance();
            ActionEditor actionEditor = (ActionEditor)node;
            _actionEditors.Add(actionEditor);
            GetNode<BoxContainer>("ScrollContainer/VerticalBoxContainer").AddChild(actionEditor);
            actionEditor.InitializeWith(this);
        }
        
        private void RemoveActionEditor(ActionEditor actionEditor)
        {
            _actionEditors.Remove(actionEditor);
            actionEditor.QueueFree();
        }
        #endregion
    }
}