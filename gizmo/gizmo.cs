//Description : gizmosCube.cs : Use to display a Cube on gameObject 
using UnityEngine;
using System.Collections;

public class gizmo : MonoBehaviour {

	public Color GizmoColor = new Color(1,0,0,.5f);		
	public bool b_Size = true;
	public Vector3 GizmoSize = new Vector3 (.025f,.025f,.025f);		// Choose on the Inspector the gizmo size

	void OnDrawGizmos()
	{
		Gizmos.color = GizmoColor;																						

		Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);		// Allow the gizmo to fit the position, rotation and scale of the gameObject
		Gizmos.matrix = cubeTransform;

		if (b_Size) {
			Gizmos.DrawCube (Vector3.zero, GizmoSize);
			Gizmos.DrawWireCube (Vector3.zero, GizmoSize);
		} else {
			Gizmos.DrawCube (Vector3.zero, Vector3.one);
			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);
		}
	}
}
