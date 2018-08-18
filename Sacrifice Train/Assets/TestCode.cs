using UnityEngine;

public class TestCode : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;

        if (Input.GetKeyDown(KeyCode.A))
            transform.position = new Vector3(x - 1, y, z);
        else if (Input.GetKeyDown(KeyCode.D))
            transform.position = new Vector3(x + 1, y, z);

        if (Input.GetKeyDown(KeyCode.X))
            enabled = !enabled;
    }
}
