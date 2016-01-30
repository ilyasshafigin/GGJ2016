using UnityEngine;
using System.Collections;

/**
 * Класс управления состоянием растения 
 */
public class PlantManager : MonoBehaviour {

	// Префаб растения
	public static Object prefab = Resources.Load ("Plant");

	// Типы растения: ничего, дерево, сорник
	public enum Type {
		Tree, Weed, None
	}

	// Тип растения
	public Type type;
	// Процесс роста от 0 до 1
	private float growth;
	// Скорость роста
	private float growthSpeed = 0.001f;
	// Процесс увядания от 0 до 1
	private float withering;
	// Скорость увядания
	private float witheringSpeed = 0.005f;
	// Температура земли
	private float temp;
	// На солнечной стороне ли растение
	private bool sun;

	// Метод инициализации при создании объекта из префаба
	private void Awake () {
		this.SetType (Type.None);
		this.growth = 0.0f;
		this.withering = 0.0f;
		this.sun = false;
	}

	// Устанавливает тип растению
	public void SetType(Type type) {
		this.type = type;
		this.gameObject.SetActive (type != Type.None);
	}
		
	public bool IsNone() {
		return this.type == Type.None;
	}

	public bool IsTree() {
		return this.type == Type.Tree;
	}

	public bool IsWeed() {
		return this.type == Type.Weed;
	}

	public void SetTemp(float temp) {
		this.temp = temp;
	}

	public void setSun(bool flag) {
		this.sun = flag;
	}
	
	// Обновление объекта на каждом каждре
	private void Update () {
		this.UpdateState ();
	}

	private void UpdateState() {
		if (this.growth < 1.0f) {
			if (this.sun) {
				if (this.withering > 0) {
					this.withering -= this.witheringSpeed;
				} else {
					this.growth += this.growthSpeed;
				}
			} else {
				this.withering += this.witheringSpeed;

				if (this.withering >= 1.0f) {
					//..
				}
			}
				
			if (this.growth >= 1.0f) {
				//...
			}
		} else {
			if (this.withering < 1.0f) {
				this.withering += this.witheringSpeed;
			}
		}
	}

	// Создает объект растения из префаба
	public static PlantManager CreatePlant() {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		PlantManager plant = gameObject.GetComponent<PlantManager> ();
		return plant;
	}

}
