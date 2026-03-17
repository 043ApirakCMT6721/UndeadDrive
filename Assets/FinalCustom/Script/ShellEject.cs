using UnityEngine;

public class ShellEject : MonoBehaviour
{
    public GameObject shellPrefab;
    public Transform ejectPoint;
    public float ejectForce = 2f;

    public void Eject()
    {
        GameObject shell = Instantiate(shellPrefab, ejectPoint.position, ejectPoint.rotation);

        Rigidbody rb = shell.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(ejectPoint.right * ejectForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * ejectForce);
        }

        Destroy(shell, 5f);
    }
}