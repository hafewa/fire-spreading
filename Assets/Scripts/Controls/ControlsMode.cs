namespace FireSimulation.Controls
{
    public enum InteractionMode
    {
        Add, // - LMB adds a plant at the clicked point with terrainLayerMask
        Remove, // - LMB removes the plant under the mouse pointer
        Toggle, // Fire - LMB lights / extinguishes fire on the plant
    }
}