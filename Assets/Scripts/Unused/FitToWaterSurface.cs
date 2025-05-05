using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class FitToWaterSurface : MonoBehaviour
{
    public WaterSurface targetSurface = null;

    // Internal search params
    WaterSearchParameters searchParameters = new WaterSearchParameters();
    WaterSearchResult searchResult = new WaterSearchResult();

    // from here:
    // https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@17.2/manual/float-objects-on-a-water-surface.html

    // Update is called once per frame
    void Update()
    {
        Debug.Log("stuff");
        if (targetSurface != null)
        {
            // Build the search parameters
            searchParameters.startPositionWS = searchResult.candidateLocationWS;
            //searchParameters.targetPositionWS = gameObject.transform.position;
            searchParameters.error = 0.01f;
            searchParameters.maxIterations = 8;
            Debug.Log("start pos: " + searchParameters.startPositionWS);

            // Do the search
            if (targetSurface.ProjectPointOnWaterSurface(searchParameters, out searchResult))
            {
                Debug.Log(searchResult.projectedPositionWS);
                gameObject.transform.position = searchResult.projectedPositionWS;
            }
            else Debug.LogError("Can't Find Projected Position");
        }
    }
}