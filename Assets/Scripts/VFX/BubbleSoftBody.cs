using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Methods below sourced from: https://github.com/dame-time/Shapes/tree/master
public class BubbleSoftBody : MonoBehaviour
{
    //TO DO LIST
    //Spawn in STATIC points based on verticies (START)
    //Spawn in DYNAMIC points based on verticies that are spring jointed to the static points (START)
    //Update the position of STATIC points based on current bubble position (UPDATE)
    //Render the Sphere mesh using the DYNAMIC points (UPDATE)

	//Fields
    //Sphere Generation
	[Header("Resolution")]
	[Range(0, 5)] [SerializeField] private int subdivisions = 1;
	[Header("Graphics")]
	private GameObject sphereMesh;
	private IcosahedronGenerator icosahedron;
	private int lastSubdivision = int.MinValue;
    [SerializeField] Material material;

    //SoftBody
    [SerializeField] GameObject staticPrefab;
    [SerializeField] GameObject dynamicPrefab;
    List<GameObject> staticVerticies = new List<GameObject>();
    List<GameObject> dynamicVerticies = new List<GameObject>();
    Rigidbody bubbleRb;

	// Start is called before the first frame update
	void Start()
	{
		//Get references
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = GetComponent<MeshFilter>().mesh;
        bubbleRb = FindObjectOfType<Bubble>().GetComponent<Rigidbody>();

		//Remove existing mesh data
		mesh.Clear();

		//Generate Sphere
		GenerateMesh();

        //Create the RBs that will form the softbody
        SpawnRigidbodies();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
        MoveRigidbodies();
    }

    //Create the static verticies on start
    void SpawnRigidbodies()
    {
        IcosahedronGenerator staticVertsGen = new IcosahedronGenerator();
        staticVertsGen.Initialize(-transform.position);
        staticVertsGen.Subdivide(subdivisions);

        int vertexCount = icosahedron.Polygons.Count * 3;
        Vector3[] vertices = new Vector3[vertexCount];

        //Create the initial Mesh shape
        for (int i = 0; i < staticVertsGen.Polygons.Count; i++)
        {
            var poly = staticVertsGen.Polygons[i];

            vertices[i * 3 + 0] = staticVertsGen.Vertices[poly.vertices[0]] + transform.position;
            vertices[i * 3 + 1] = staticVertsGen.Vertices[poly.vertices[1]] + transform.position;
            vertices[i * 3 + 2] = staticVertsGen.Vertices[poly.vertices[2]] + transform.position;
        }

        //Create transform points
        for (int i = 0; i < staticVertsGen.Vertices.Count; i++)
        {
            //We use staticVertsGen here because we want to get the positions before they're rearranged but this loop up top
            staticVerticies.Add(Instantiate(staticPrefab, staticVertsGen.Vertices[i], Quaternion.identity));
            dynamicVerticies.Add(Instantiate(dynamicPrefab, staticVertsGen.Vertices[i], Quaternion.identity));
            dynamicVerticies[i].GetComponent<VFXJoint>().ConnectedBody = staticVerticies[i].GetComponent<Rigidbody>();
            //dynamicVerticies[i].GetComponent<VFXJoint>().ConnectedBody = bubbleRb;
            //dynamicVerticies[i].GetComponent<Rigidbody>().mass = Random.Range(0.95f, 1.05f);
        }
    }
    
    //Move the verticies every frame update to coincide with the position of the object
    void MoveRigidbodies()
    {
        IcosahedronGenerator staticVertsGen = new IcosahedronGenerator();
        staticVertsGen.Initialize(Vector3.zero);
        staticVertsGen.Subdivide(subdivisions);

        int vertexCount = icosahedron.Polygons.Count * 3;
        Vector3[] vertices = new Vector3[vertexCount];

        //Create the initial Mesh shape
        for (int i = 0; i < staticVertsGen.Polygons.Count; i++)
        {
            var poly = staticVertsGen.Polygons[i];

            vertices[i * 3 + 0] = icosahedron.Vertices[poly.vertices[0]] + transform.position;
            vertices[i * 3 + 1] = icosahedron.Vertices[poly.vertices[1]] + transform.position;
            vertices[i * 3 + 2] = icosahedron.Vertices[poly.vertices[2]] + transform.position;
        }

        //Move transform points
        for (int i = 0; i < staticVertsGen.Vertices.Count; i++)
        {
            staticVerticies[i].GetComponent<Rigidbody>().MovePosition(staticVertsGen.Vertices[i] + bubbleRb.position);
        }

        List<Vector3> verticiesUpdated = new List<Vector3>();
        for(int i = 0; i < staticVertsGen.Vertices.Count; i++)
        {
            verticiesUpdated.Add(dynamicVerticies[i].transform.position - transform.position); //Take away position offset
        }


        //START OVER

        this.name = "IcoSphere";

        if (this.sphereMesh)
            Destroy(this.sphereMesh);

        icosahedron = new IcosahedronGenerator();
        icosahedron.Initialize(verticiesUpdated);
        icosahedron.Subdivide(subdivisions);

        this.sphereMesh = new GameObject("Sphere Mesh");
        this.sphereMesh.transform.parent = this.transform;

        MeshRenderer surfaceRenderer = this.sphereMesh.AddComponent<MeshRenderer>();
        //surfaceRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        surfaceRenderer.sharedMaterial = material;

        Mesh sphereMesh = new Mesh();

        vertexCount = icosahedron.Polygons.Count * 3;
        int[] indices = new int[vertexCount];

        vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];

        for (int i = 0; i < icosahedron.Polygons.Count; i++)
        {
            var poly = icosahedron.Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = icosahedron.Vertices[poly.vertices[0]] + transform.position;
            vertices[i * 3 + 1] = icosahedron.Vertices[poly.vertices[1]] + transform.position;
            vertices[i * 3 + 2] = icosahedron.Vertices[poly.vertices[2]] + transform.position;

            normals[i * 3 + 0] = icosahedron.Vertices[poly.vertices[0]] + transform.position;
            normals[i * 3 + 1] = icosahedron.Vertices[poly.vertices[1]] + transform.position;
            normals[i * 3 + 2] = icosahedron.Vertices[poly.vertices[2]] + transform.position;
        }
        sphereMesh.vertices = vertices;
        sphereMesh.normals = normals;
        sphereMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = this.sphereMesh.AddComponent<MeshFilter>();
        terrainFilter.sharedMesh = sphereMesh;
    }

	public void GenerateMesh()
	{
		this.name = "IcoSphere";

		if (this.sphereMesh)
			Destroy(this.sphereMesh);

		icosahedron = new IcosahedronGenerator();
		icosahedron.Initialize(transform.position);
		icosahedron.Subdivide(subdivisions);

		this.sphereMesh = new GameObject("Sphere Mesh");
		this.sphereMesh.transform.parent = this.transform;

		MeshRenderer surfaceRenderer = this.sphereMesh.AddComponent<MeshRenderer>();
		surfaceRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

		Mesh sphereMesh = new Mesh();

		int vertexCount = icosahedron.Polygons.Count * 3;
		int[] indices = new int[vertexCount];

		Vector3[] vertices = new Vector3[vertexCount];
		Vector3[] normals = new Vector3[vertexCount];

		for (int i = 0; i < icosahedron.Polygons.Count; i++)
		{
			var poly = icosahedron.Polygons[i];

			indices[i * 3 + 0] = i * 3 + 0;
			indices[i * 3 + 1] = i * 3 + 1;
			indices[i * 3 + 2] = i * 3 + 2;

			vertices[i * 3 + 0] = icosahedron.Vertices[poly.vertices[0]] + transform.position;
			vertices[i * 3 + 1] = icosahedron.Vertices[poly.vertices[1]] + transform.position;
			vertices[i * 3 + 2] = icosahedron.Vertices[poly.vertices[2]] + transform.position;

			normals[i * 3 + 0] = icosahedron.Vertices[poly.vertices[0]] + transform.position;
			normals[i * 3 + 1] = icosahedron.Vertices[poly.vertices[1]] + transform.position;
			normals[i * 3 + 2] = icosahedron.Vertices[poly.vertices[2]] + transform.position;
		}
		sphereMesh.vertices = vertices;
		sphereMesh.normals = normals;
		sphereMesh.SetTriangles(indices, 0);

		MeshFilter terrainFilter = this.sphereMesh.AddComponent<MeshFilter>();
		terrainFilter.sharedMesh = sphereMesh;
	}

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        for (int i = 0; i < dynamicVerticies.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(dynamicVerticies[i].transform.position, 0.05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(staticVerticies[i].transform.position, 0.05f);
        }
    }
}

public class IcosahedronGenerator
{
    private List<Polygon> polygons;
    private List<Vector3> vertices;

    public List<Polygon> Polygons { get => polygons; private set => polygons = value; }
    public List<Vector3> Vertices { get => vertices; private set => vertices = value; }

    public void Initialize(Vector3 worldPos) //Need to subtract worldposition when you spawn the initial points
    {
        polygons = new List<Polygon>();
        vertices = new List<Vector3>();

        // An icosahedron has 12 vertices, and
        // since it's completely symmetrical the
        // formula for calculating them is kind of
        // symmetrical too:

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        vertices.Add(new Vector3(-1, t, 0).normalized - worldPos);
        vertices.Add(new Vector3(1, t, 0).normalized - worldPos);
        vertices.Add(new Vector3(-1, -t, 0).normalized - worldPos);
        vertices.Add(new Vector3(1, -t, 0).normalized - worldPos);
        vertices.Add(new Vector3(0, -1, t).normalized - worldPos);
        vertices.Add(new Vector3(0, 1, t).normalized - worldPos);
        vertices.Add(new Vector3(0, -1, -t).normalized - worldPos);
        vertices.Add(new Vector3(0, 1, -t).normalized - worldPos);
        vertices.Add(new Vector3(t, 0, -1).normalized - worldPos);
        vertices.Add(new Vector3(t, 0, 1).normalized - worldPos);
        vertices.Add(new Vector3(-t, 0, -1).normalized - worldPos);
        vertices.Add(new Vector3(-t, 0, 1).normalized - worldPos);


        // And here's the formula for the 20 sides,
        // referencing the 12 vertices we just created.
        polygons.Add(new Polygon(0, 11, 5));
        polygons.Add(new Polygon(0, 5, 1));
        polygons.Add(new Polygon(0, 1, 7));
        polygons.Add(new Polygon(0, 7, 10));
        polygons.Add(new Polygon(0, 10, 11));
        polygons.Add(new Polygon(1, 5, 9));
        polygons.Add(new Polygon(5, 11, 4));
        polygons.Add(new Polygon(11, 10, 2));
        polygons.Add(new Polygon(10, 7, 6));
        polygons.Add(new Polygon(7, 1, 8));
        polygons.Add(new Polygon(3, 9, 4));
        polygons.Add(new Polygon(3, 4, 2));
        polygons.Add(new Polygon(3, 2, 6));
        polygons.Add(new Polygon(3, 6, 8));
        polygons.Add(new Polygon(3, 8, 9));
        polygons.Add(new Polygon(4, 9, 5));
        polygons.Add(new Polygon(2, 4, 11));
        polygons.Add(new Polygon(6, 2, 10));
        polygons.Add(new Polygon(8, 6, 7));
        polygons.Add(new Polygon(9, 8, 1));
    }

    //Alternate version of Initalize where I feed in the verticies
    public void Initialize(List<Vector3> newVertices)
    {
        polygons = new List<Polygon>();
        vertices = newVertices;

        // And here's the formula for the 20 sides,
        // referencing the 12 vertices we just created.
        polygons.Add(new Polygon(0, 11, 5));
        polygons.Add(new Polygon(0, 5, 1));
        polygons.Add(new Polygon(0, 1, 7));
        polygons.Add(new Polygon(0, 7, 10));
        polygons.Add(new Polygon(0, 10, 11));
        polygons.Add(new Polygon(1, 5, 9));
        polygons.Add(new Polygon(5, 11, 4));
        polygons.Add(new Polygon(11, 10, 2));
        polygons.Add(new Polygon(10, 7, 6));
        polygons.Add(new Polygon(7, 1, 8));
        polygons.Add(new Polygon(3, 9, 4));
        polygons.Add(new Polygon(3, 4, 2));
        polygons.Add(new Polygon(3, 2, 6));
        polygons.Add(new Polygon(3, 6, 8));
        polygons.Add(new Polygon(3, 8, 9));
        polygons.Add(new Polygon(4, 9, 5));
        polygons.Add(new Polygon(2, 4, 11));
        polygons.Add(new Polygon(6, 2, 10));
        polygons.Add(new Polygon(8, 6, 7));
        polygons.Add(new Polygon(9, 8, 1));
    }

    public void Subdivide(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in polygons)
            {
                int a = poly.vertices[0];
                int b = poly.vertices[1];
                int c = poly.vertices[2];
                // Use GetMidPointIndex to either create a
                // new vertex between two old vertices, or
                // find the one that was already created.
                int ab = GetMidPointIndex(midPointCache, a, b);
                int bc = GetMidPointIndex(midPointCache, b, c);
                int ca = GetMidPointIndex(midPointCache, c, a);
                // Create the four new polygons using our original
                // three vertices, and the three new midpoints.
                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }
            // Replace all our old polygons with the new set of
            // subdivided ones.
            polygons = newPolys;
        }
    }

    public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
    {
        // We create a key out of the two original indices
        // by storing the smaller index in the upper two bytes
        // of an integer, and the larger index in the lower two
        // bytes. By sorting them according to whichever is smaller
        // we ensure that this function returns the same result
        // whether you call
        // GetMidPointIndex(cache, 5, 9)
        // or...
        // GetMidPointIndex(cache, 9, 5)
        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;
        // If a midpoint is already defined, just return it.
        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;
        // If we're here, it's because a midpoint for these two
        // vertices hasn't been created yet. Let's do that now!
        Vector3 p1 = vertices[indexA];
        Vector3 p2 = vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = vertices.Count;
        vertices.Add(middle);

        cache.Add(key, ret);
        return ret;
    }
}

public class Polygon
{
	public readonly List<int> vertices;

	public Polygon(int a, int b, int c)
	{
		vertices = new List<int>() { a, b, c };
	}
}
