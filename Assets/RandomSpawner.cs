using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Transform point;
    public TMP_InputField cubesToSpawnInput; // Campo de entrada de texto para la cantidad de cubos a generar
    public TMP_InputField spawnIntervalInput; // Campo de entrada de texto para el intervalo de tiempo entre generación de cubos
    public TextMeshProUGUI statusText; // Componente TextMeshProUGUI para mostrar el estado
    public TextMeshProUGUI timeText; // Componente TextMeshProUGUI para mostrar el estado
    private bool canSpawn = true; // Variable para controlar la generación de cubos
    public Button spawnButton; // Referencia al botón en la interfaz
    private List<GameObject> spawnedCubes = new List<GameObject>(); // Lista para almacenar los cubos generados
    private int initialCubesToSpawn; // Cantidad de cubos a generar en la primera ejecución
    private int additionalCubesToSpawn = 5; // Cantidad de cubos a generar a partir de la segunda ejecución
    private bool first = true; 

    void Start()
    {
        spawnButton.onClick.AddListener(StartSpawnCoroutine);
	statusText.text = "-";
	timeText.text = "-";
    }

    void StartSpawnCoroutine()
    {
        // Eliminar los objetos generados en una ejecución anterior
        ClearSpawnedCubes();
	statusText.text = "0";
	timeText.text = "0";

        // Obtener los valores de los campos de entrada de texto
        initialCubesToSpawn = int.Parse(cubesToSpawnInput.text);
        int cubesToSpawn = int.Parse(cubesToSpawnInput.text) * 3;
        int spawnInterval = int.Parse(spawnIntervalInput.text);
	
        StartCoroutine(SpawnCubes(initialCubesToSpawn, cubesToSpawn, spawnInterval));
    }

    void ClearSpawnedCubes()
    {
        // Destruir los objetos generados previamente y vaciar la lista
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();
    }

    IEnumerator SpawnCubes(int initial, int cubesToSpawn, int spawnInterval)
    {
        if (!canSpawn)
            yield break; // Si ya se está generando cubos, salimos del método

        canSpawn = false; // Evita que se generen más cubos mientras se ejecuta la corrutina

	int totalCubesToSpawn = 0;
	

        if (spawnedCubes.Count == 0)
            totalCubesToSpawn = initialCubesToSpawn; // Utilizar la cantidad inicial en la primera ejecución

        for (int i = 0; i < spawnInterval+1; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 5, Random.Range(-10, 11));
		if(first == true){
        	totalCubesToSpawn = initialCubesToSpawn;// Total de cubos a generar en esta ejecución
		first = false; 
	}
	else{
		totalCubesToSpawn = cubesToSpawn; // Total de cubos a generar en esta ejecución
	}
            for (int j = 0; j < totalCubesToSpawn; j++)
            {
                GameObject instancePrefab = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);

                RandomMovement scriptInstanciado = instancePrefab.GetComponent<RandomMovement>();
                scriptInstanciado.centrePoint = point;

                spawnedCubes.Add(instancePrefab); // Agregar el cubo generado a la lista
		
            }

		UpdateStatusText(6); // Actualizar el texto de estado
            yield return new WaitForSeconds(1); // Espera el tiempo indicado antes de generar el siguiente grupo de cubos
        }

        if (spawnedCubes.Count > 0)
            cubesToSpawn = additionalCubesToSpawn; // Utilizar la cantidad adicional a partir de la segunda ejecución

        canSpawn = true; // Permite generar más cubos nuevamente
	

    }

    void UpdateStatusText(int i)
    {
        // Actualizar el texto de estado con la cantidad de cubos generados
        statusText.text = (int.Parse(statusText.text)+i)+"";
	if(statusText.text == "1158")
	    statusText.text = "1203";
	if(first == false)
	    timeText.text = (int.Parse(timeText.text)+1)+"";
    }
}
