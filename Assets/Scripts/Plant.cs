using UnityEngine;
using System.Collections;

/**
 * Класс управления состоянием растения 
 */
public class Plant : MonoBehaviour {

	// Префаб растения
	public static Object prefab = Resources.Load ("Prefabs/Plant");

	// Типы растения: ничего, дерево, сорник
	public enum Type {
		None, Tree, Weed
	}

	// Состояния растения
	public enum State {
		None, 			// ничего
		Growth,			// рост
		Divide,			// деление
		Withreing,		// увядание
		WithreingDone,	// увяло
		DryingUp, 		// засыхание
		DryingUpDone,	// засохло
		Burn,			// горение
		BurnDone,		// сгорело
		Explosion,		// взрыв
		Die				// умирание
	}

	// Тип растения
	private Type type = Type.None;
	// Состояние
	public State state = State.None;

	// Процесс роста от 0 до 1
	public float growth = 0.0f;
	// Скорость роста
	public float growthSpeed = 0.15f;
	// Процесс деления от 0 до 1
	private float divide = 0.0f;
	// Скорость деления
	public float divideSpeed = 1f;
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
	public float burnSpeed = 0.15f;
	// Прогресс умирания, необратим
	private float die = 0.0f;
	//	Скорость умирания
	public float dieSpeed = 0.4f;
	// Температура земли
	private float temp = 0.0f;
	// На солнечной стороне ли растение
	private bool sun = false;

	// Начальный цвет увядания
	public Color witheringColorFrom = new Color(1.0f, 1.0f, 1.0f);
	// Конечный цвет увядания
	public Color witheringColorTo = new Color(0.3f, 0.3f, 1.0f);

	// Начальный цвет перегревания
	public Color burnColorFrom = new Color(1.0f, 1.0f, 1.0f);
	// Конечный цвет перегревания
	public Color burnColorTo = new Color(1.0f, 0.3f, 0.3f);

	public AudioClip growSound;
	public AudioClip fireSound;

	private Planet planet;
	private int index;
	private Animator animator;
	private SpriteRenderer render;

	// Метод инициализации
	private void Awake () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
		this.render = this.GetComponent<SpriteRenderer> ();
	}

	// Метод инициализации при создании объекта из префаба
	private void Initialize(Planet planet, int index) {
		this.planet = planet;
		this.index = index;
	}

	// Устанавливает тип растению
	public void SetType(Type type) {
		this.type = type;
		if (this.IsNone()) {
			this.Hide ();
		} else {
			this.Show ();
		}
	}

	// Устанавивает состояние
	public void SetState(State state) {
		this.state = state;
		if (state == State.Growth) {
			this.OnGrowth ();
		}
	}

	//
	public bool IsNone() {
		return this.type == Type.None;
	}

	//
	public bool IsTree() {
		return this.type == Type.Tree;
	}

	//
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
		if (this.IsNone())
			return;

		if (GameManager.instance.IsGameOver ())
			return;

		switch (this.state) {
		// Если дерево вянет
		case State.Withreing: {
				// Если дерево освещено
				if (this.sun) {
					// то убираем прогресс увядания
					this.withering -= 1.5f*this.witheringSpeed * Time.deltaTime;
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
					//if (this.growth <= 0.5f) {
					//	this.growth += this.growthSpeed * Time.deltaTime;
					//}
				}
				// Устанавливаем кадр анимации
				this.animator.Play ("GrowthTree", 0, this.growth);
				//this.render.color = Color.Lerp (this.witheringColorFrom, this.witheringColorTo, this.withering);
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
					// Запускаем событие
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
				this.render.color = Color.white;
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
						this.SetState (State.Die);
						// Останавливаем звук
						this.GetComponent<AudioSource> ().Stop ();
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
						// Останавливаем звук
						this.GetComponent<AudioSource> ().Stop ();
					}
				}

				if (this.growth >= 0.4f) {
					// Устанавливаем кадр анимации
					this.animator.Play ("BurnTree", 0, this.burn);
				} else {
					// Устанавливаем кадр анимации
					this.animator.Play ("BurnTreeMini", 0, this.burn);
				}
				//this.render.color = Color.Lerp (this.burnColorFrom, this.burnColorTo, this.burn);
				break;
		}
		//
		case State.Die: {
				// 
				this.die += this.dieSpeed * Time.deltaTime;
				// 
				if (this.die >= 1.0f) {
					this.die = 1.0f;
					// Устанавливаем состояние
					this.OnDieDone();
				}
				// Управляем анимацией
				if (this.growth >= 0.6f) {
					this.animator.Play ("DieTree", 0, this.die);
				} else if (this.growth >= 0.4f) {
					this.animator.Play ("DieTreeMid", 0, this.die);
				} else {
					this.animator.Play ("DieTreeMMini", 0, this.die);
				}
				break;
		}
		default:
			break;
		}
			
		if (this.state != State.Divide && this.state != State.Explosion && this.state != State.Die) {
			// Если температура большая
			if (this.temp >= 0.99f && this.state != State.Burn) {
				// то устанавливаем состояние горения
				//this.SetState (State.Burn);
				this.OnBurn();
			}
			// Иначе, если температура низкая
			else if (this.temp <= -0.5f && this.state != State.Withreing) {
				// то устанавливаем состояние увядания
				this.SetState (State.Withreing);
			}
		}

		// TODO debug
		//this.render.color = Color.Lerp (Color.blue, Color.red, this.temp/2+0.5f);
	}

	// Событие начала роста
	public void OnGrowth() {

	}

	// Событие окончания роста и деления дерева
	public void OnDivide() {
		AudioSource audio = this.GetComponent<AudioSource> ();
		// Запускаем звук
		audio.Stop ();
		audio.loop = false;
		audio.clip = this.growSound;
		audio.Play ();

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
		// Останавливаем звук
		this.GetComponent<AudioSource> ().Stop ();
		// Устанавливаем состояние
		this.SetType (Type.None);
	}

	// Событие загорания
	public void OnBurn() {
		if (this.IsNone ())
			return;
		if (this.state == State.Die)
			return;

		this.SetState (State.Burn);

		// Запускаем звук
		AudioSource audio = this.GetComponent<AudioSource> ();
		audio.Stop ();
		audio.loop = true;
		audio.clip = this.fireSound;
		audio.Play ();
	}

	// Событие распространения пожара
	public void OnFire() {
		this.planet.OnFire (this.index);
	}

	//
	public void OnDieDone() {
		// Останавливаем звук
		this.GetComponent<AudioSource> ().Stop ();
		// Удаляем дерево
		this.SetType (Type.None);
	}

	// Событие запуска игры
	public void OnStartGame() {

	}

	// Событие окончания игры
	public void OnGameOver() {
		this.SetTemp (0);
		this.SetType (Plant.Type.None);
		this.SetGrowth (0);
		this.SetState (Plant.State.None);
	}

	// Делит растение на еще два
	private void Divide() {
		int count = 2;
		while (count-- > 0) {
			// Спауним Растение
			this.planet.Spawn();
			// Добавляем очки
			GameManager.instance.AddScore(1);
		}
	}

	// Показывает объект
	private void Show() {
		this.GetComponent<SpriteRenderer> ().enabled = true;
	}

	// Скрывает объект
	private void Hide() {
		this.GetComponent<SpriteRenderer> ().enabled = false;
	}

	// Создает объект растения из префаба
	public static Plant CreatePlant(Planet planet, int index) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		Plant plant = gameObject.GetComponent<Plant> ();
		plant.Initialize (planet, index);
		return plant;
	}

}
