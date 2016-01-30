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

	// Состояния растения: ничего, рост, полность выросло, увядание, увяло, засыхание, засохло, горит, сгорело
	public enum State {
		None, Growth, GrowthDone, Withreing, WithreingDone, DryingUp, DryingUpDone, Burn, BurnDone
	}

	// Тип растения
	public Type type;
	// Состояние
	public State state;

	// Процесс роста от 0 до 1
	private float growth = 0.0f;
	// Скорость роста
	private float growthSpeed = 0.01f;
	// Процесс увядания от 0 до 1
	private float withering = 0.0f;
	// Скорость увядания
	private float witheringSpeed = 0.05f;
	// Процесс засыхания дерева от 0 до 1
	private float dryingUp = 0.0f;
	// Скорость засыхания дерева
	private float dryingUpSpeed = 0.05f;
	// Процесс горения от 0 до 1
	private float burn = 0.0f;
	// Скорость горения
	private float burnSpeed = 0.05f;
	// Температура земли
	private float temp = 0.0f;
	// На солнечной стороне ли растение
	private bool sun = false;

	private PlanetManager planet;
	private int index;
	private Animator animator;

	// Метод инициализации
	private void Awake () {
		this.SetType (Type.None);
		this.SetState (State.Growth);
		this.animator = this.GetComponent<Animator> ();
	}

	// Метод инициализации при создании объекта из префаба
	private void Initialize(PlanetManager planet, int index) {
		this.planet = planet;
		this.index = index;
	}

	// Устанавливает тип растению
	public void SetType(Type type) {
		this.type = type;
		this.gameObject.SetActive (type != Type.None);
	}

	// Устанавивает состояние
	public void SetState(State state) {
		this.state = state;
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

	// Устанавливает температуру дерева
	public void SetTemp(float temp) {
		this.temp = temp;
	}

	// Определяет, освещается ли растение
	public void setSun(bool flag) {
		this.sun = flag;
	}
	
	// Обновление объекта на каждом каждре
	private void Update () {
		this.UpdateState ();
	}

	private void UpdateState() {
		switch (this.state) {
		// Если дерево вянет
		case State.Withreing: {
				// Если дерево освещено
				if (this.sun) {
					// то убираем прогресс увядания
					this.withering -= this.witheringSpeed;
					// Если процесс полность убрали
					if (this.withering <= 0.0f) {
						// Устанавливаем состояние роста
						this.SetState(State.Growth);
					}
				}
				// Иначе не освящено
				else {
					// Продолжаем процесс увядания
					this.withering += this.witheringSpeed;
					// Если дерево полность увяло
					if (this.withering >= 1.0f) {
						// то устанавливаем состояние
						this.SetState(State.WithreingDone);
						// и запускаем событие
						this.OnWitheringDone();
					}
				}
				break;
			}
		// Если дерево еще растет
		case State.Growth: {
				// Если дерево освещено
				if (this.sun) {
					// иначе продолжаем рост
					this.growth += this.growthSpeed;
					this.animator.Play ();
					// Если дерево полность выросло
					if (this.growth >= 1.0f) {
						// то устанавляваем состояние
						this.SetState(State.GrowthDone);
						// и запускаем событие деления
						this.OnGrowthDone ();
					}
				}
				// иначе не освещено
				else {
					// Устанавливаем состояние увядания
					this.SetState (State.Withreing);
				}
				break;
		}
		// Если дерево полность выросло
		case State.GrowthDone: {
				// Устанавлмваем состояние засыхания
				this.SetState (State.DryingUp);
				break;
		}
		// Если дерево сохнет
		case State.DryingUp: {
				// Продолжаем процесс засыхания
				this.dryingUp += this.dryingUpSpeed;
				// Если дерево полность засохло
				if (this.dryingUp >= 1.0f) {
					// Устанавливаем состояние
					this.SetState (State.DryingUpDone);
				}
				break;
		}
		// Если дерево горит
		case State.Burn: {
				// Если объект на солнечной стороне
				if (this.sun) {
					// то продолжаем процесс горения
					this.burn += this.burnSpeed;
					// Если дерево сгорело на половину
					if(this.burn >= 0.5f) {
						// то запускаем событие перехода пожара на соседние деревья
						this.OnFire();
					}
					// Если дерево полность сгорело
					if (this.burn >= 1.0f) {
						// то устанавливаем состояние полного сгорания
						this.SetState (State.BurnDone);
						// и запускаем событие
						this.OnBurn();
					}
				}
				// Иначе
				else {
					// идем в обратную сторону
					this.burn -= this.burnSpeed;
					// Если дерево перестало гореть
					if (this.burn <= 0.0f) {
						// Устанавливаем состояние роста
						this.SetState(State.Growth);
					}
				}
				break;
		}
		default:
			break;
		}

		if (this.temp >= 1.0f) {
			this.SetState (State.Burn);
		}
	}

	// Событие начала роста
	private void OnGrowth() {
	}

	// Событие окончания роста и деления дерева
	private void OnGrowthDone() {
		int count = 2;
		while (count-- > 0) {
			this.planet.Spawn();
		}
	}

	// Событие гибели дерева от увядания
	private void OnWitheringDone() {
		
	}

	// Событие столкновения с кометой
	private void OnComet() {
		
	}

	// Событие полного сгорания дерева
	private void OnBurn() {
		// Удаляем дерево
		this.SetType (Type.None);
	}

	// Событие распространения пожара
	private void OnFire() {
		this.planet.OnFire (this.index);
	}

	// Создает объект растения из префаба
	public static PlantManager CreatePlant(PlanetManager planet, int index) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		PlantManager plant = gameObject.GetComponent<PlantManager> ();
		plant.Initialize (planet, index);
		return plant;
	}

}
