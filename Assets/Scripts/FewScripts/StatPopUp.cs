using TMPro;
using UnityEngine;

public class StatPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;

    public string popUpText = "Null";
    public Color popUpColor = Color.white;

    [SerializeField] private float floatUpStatSpeed = 2f;
    [SerializeField] private float statTextTime = 0f;
    [SerializeField] private float statTextDuration = 1f; // how long its staying until it goes away. if its statTextDuration = 1, then it go poof at 1 second

    void Start()
    {
        if (statText == null)
        {
            Debug.LogError("StatPopUp: statText is not assigned!");
            return;
        }
        
        statText.text = popUpText;
        statText.color = popUpColor;
    }
    void Update()
    {
        transform.position += new Vector3(0, floatUpStatSpeed * Time.deltaTime, 0);
        statTextTime += Time.deltaTime;

        if(statTextTime >= statTextDuration)
        {
            Destroy(gameObject);
        }
    }
}
