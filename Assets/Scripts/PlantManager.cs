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
		None, Tree, Weed
	}

	// Состояния растения: ничего, рост, деление, увядание, увяло, засыхание, засохло, горит, сгорело
	public enum State {
		None, Growth, Divide, Withreing, WithreingDone, DryingUp, DryingUpDone, Burn, BurnDone, Explosion
	}

	// Тип растения
	private Type type = Type.None;
	// Состояние
	public State state = State.None;

	// Процесс роста от 0 до 1
	public float growth = 0.0f;
	// Скорость роста
	public float growthSpeed = 0.1f;
	// Процесс деления от 0 до 1
	private float divide = 0.0f;
	// Скорость деления
	private float divideSpeed = 1f;
	// Процесс увядания от 0 до 1
	private float withering = 0.0f;
	// Скорость увядания
	public float witheringSpeed = 0.1f;
	// Процесс засыхания дерева от 0 до 1
	private float dryingUp = 0.0f;
	// Скорость засыхания дерева
	public float dryingUpSpeed = 0.1f;
	// Процесс горения от 0 до 1
	private float burn = 0.0f;
	// Скорость горения
	public float burnSpeed = 0.1f;
	// Температура земли
	private float temp = 0.0f;
	// На солнечной стороне ли растение
	private bool sun = false;

	// Количество кадров роста
	public int treeAnimFrames = 24;
	// Количество кадров деления
	public int divideAnimFrames = 7;

	// Начальный цвет увядания
	public Color witheringColorFrom = new Color(1.0f, 1.0f, 1.0f);
	// Конечный цвет увядания
	public Color witheringColorTo = new Color(0.3f, 0.3f, 1.0f);

	// Начальный цвет перегревания
	public Color burnColorFrom = new Color(1.0f, 1.0f, 1.0f);
	// Конечный цвет перегревания
	public Color burnColorTo = new Color(1.0f, 0.3f, 0.3f);

	private PlanetManager planet;
	private int index;
	private Animator animator;
	private SpriteRenderer render;

	// Метод инициализации
	private void Awake () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
		this.render = this.GetComponent<SpriteRenderer> ();
	}

	// 
	private void Start() {

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
		if (state == State.Growth) {
			this.OnGrowth ();
		}
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
	public void SetSun(bool flag) {
		this.sun = flag;
	}

	// Устанавливает прогресс роста
	public void SetGrowth(float growth) {
		this.growth = growth;
	}
	
	// Обновление объекта на каждом каждре
	private void Update () {
		this.UpdateState ();
	}

	// Обновляет состояние растения
	private void UpdateState() {
		if (this.type == Type.None) {
			return;
		}

		switch (this.state) {
		// Если дерево вянет
		case State.Withreing: {
				// Если дерево освещено
				if (this.sun) {
					// то убираем прогресс увядания
					this.withering -= this.witheringSpeed * Time.deltaTime;
					// Если процесс полность убрали
					if (this.withering <= 0.0f) {
						this.withering = 0.0f;
						// Устанавливаем состояние роста
						this.SetState(State.Growth);
					}
				}
				// Иначе не освящено
				else {
					// Продолжаем процесс увядания
					this.withering += this.witheringSpeed * Time.deltaTime;
					// Если дерево полность увяло
					if (this.withering >= 1.0f) {
						this.withering = 1.0f;
						// то устанавливаем состояние
						this.SetState(State.WithreingDone);
						// и запускаем событие
						this.OnWitheringDone();
					}
				}
				// Устанавливаем кадр анимации
				this.animator.Play ("GrowthTree", 0, this.growth);
				this.render.color = Color.Lerp (this.witheringColorFrom, this.witheringColorTo, this.withering);
				break;
			}
		// Если дерево еще растет
		case State.Growth: {
				// Если дерево освещено
				if (this.sun) {
					// иначе продолжаем рост
					this.growth += this.growthSpeed * Time.deltaTime;
					// Если дерево полность выросло
					if (this.growth >= 1.0f) {
						this.growth = 1.0f;
						// то устанавляваем состояние
						this.SetState(State.Divide);
						// и запускаем событие деления
						this.OnDivide ();
					}
					// Устанавливаем кадр анимации
					this.animator.Play ("GrowthTree", 0, this.growth);
				}
				// иначе не освещено
				else {
					// Устанавливаем состояние увядания
					this.SetState (State.Withreing);
				}
				break;
		}
		// Если дерево полность выросло
		case State.Divide: {
				// Увеличиваем прогресс деления
				this.divide += this.divideSpeed * Time.deltaTime;
				// Если дерево полность поделилось
				if (this.divide >= 1.0f) {
					this.divide = 1.0f;
					// Устанавляваем состояние засыхания
					this.SetState (State.DryingUp);
					//
					this.OnDivideDone();
				}
				// Управляем анимацией
				this.animator.Play ("DivideTree", 0, this.divide);
				break;
		}
		// Если дерево сохнет
		case State.DryingUp: {
				// Продолжаем процесс засыхания
				this.dryingUp += this.dryingUpSpeed * Time.deltaTime;
				// Если дерево полность засохло
				if (this.dryingUp >= 1.0f) {
					this.dryingUp = 1.0f;
					// Устанавливаем состояние
					this.SetState (State.DryingUpDone);
				}
				// Управляем анимацией
				// TODO временно
				this.animator.Play ("DivideTree", 0, 1.0f);
				break;
		}
		// Если дерево горит
		case State.Burn: {
				// Если объект на солнечной стороне
				if (this.sun) {
					// то продолжаем процесс горения
					this.burn += this.burnSpeed * Time.deltaTime;
					// Если дерево сгорело на половину
					if (this.burn >= 0.5f) {
						// то запускаем событие перехода пожара на соседние деревья
						this.OnFire ();
					}
					// Если дерево полность сгорело
					if (this.burn >= 1.0f) {
						this.burn = 1.0f;
						// то устанавливаем состояние полного сгорания
						this.SetState (State.BurnDone);
						// и запускаем событие
						this.OnBurn ();
					}
				}
				// Иначе
				else {
					// идем в обратную сторону
					this.burn -= this.burnSpeed * Time.deltaTime;
					// Если дерево перестало гореть
					if (this.burn <= 0.0f) {
						this.burn = 0.0f;
						// Устанавливаем состояние роста
						this.SetState (State.Growth);
					}
				}
				// Устанавливаем кадр анимации
				this.animator.Play ("GrowthTree", 0, this.growth);
				this.render.color = Color.Lerp (this.burnColorFrom, this.burnColorTo, this.burn);
				break;
		}
		default:
			break;
		}

		// Если температура большая
		if (this.temp >= 1.0f && this.state != State.Divide && this.state != State.Explosion) {
			// то устанавливаем состояние горения
			this.SetState (State.Burn);
		}
	}

	// Событие начала роста
	public void OnGrowth() {

	}

	// Событие окончания роста и деления дерева
	public void OnDivide() {
		this.planet.OnGrow (this.index);
	}

	// Событие окончания деления
	public void OnDivideDone() {
		this.Divide ();
	}

	// Событие гибели дерева от увядания
	public void OnWitheringDone() {
		
	}

	// Событие столкновения с кометой
	public void OnComet() {
		this.SetType (Type.None);
	}

	// Событие полного сгорания дерева
	public void OnBurn() {
		// Удаляем дерево
		this.SetType (Type.None);
	}

	// Событие распространения пожара
	public void OnFire() {
		this.planet.OnFire (this.index);
	}

	// Делит растение на еще два
	private void Divide() {
		int count = 2;
		while (count-- > 0) {
			this.planet.Spawn();
		}
	}

	// Создает объект растения из префаба
	public static PlantManager CreatePlant(PlanetManager planet, int index) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		PlantManager plant = gameObject.GetComponent<PlantManager> ();
		plant.Initialize (planet, index);
		return plant;
	}

}
