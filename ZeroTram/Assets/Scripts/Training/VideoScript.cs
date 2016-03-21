using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private Text _cloudText;
    [SerializeField] private GameObject _all;
    [SerializeField] private GameObject _audio;
    [SerializeField] private Canvas _interface;

    public static bool _isTraining { get; set; }
    public static bool _isPause { get; set; }

    private Dictionary<int, string> phrasesByFrames = new Dictionary<int, string>();
    private string _manyHares = "Нужно было успеть выпинуть всех зайцев до следующей остановки! Попробуем ещё раз";
    private string _manyKilled = "Следи за оплатившими билет! Нужно довести их в целости и сохранности.";
    private string _conductorKilled = "Эх, совсем себя не бережёшь! В следующий раз будь осторожнее!";
    private int _currentNum = 1;
    private bool _sceneChanging;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 0;
        _isPause = true;
        phrasesByFrames.Add(1, "Незнакомое место, да?");
        phrasesByFrames.Add(2, "Не мудрено, мы - в другом мире. Никогда не знаешь, куда занесёт этот трамвай");
        phrasesByFrames.Add(3, "А чтобы такие, как ты, могли попасть туда, куда хотят, трамваю нужен кондуктор. Но я, пожалуй, останусь в твоём мире. Поэтому теперь искать мир себе по душе будешь ты. Но сначала тебе нужно освоиться. В путь!");
        phrasesByFrames.Add(4, "В трамвае новый пассажир. Скорее возьми с него плату за проезд!");
        phrasesByFrames.Add(5, "Отлично! Теперь у нас есть проданный билет. Но не все пассажиры хотят их покупать. О, скоро уже следующая остановка");
        phrasesByFrames.Add(6, "Похоже, за билет платить он не собирается. Можешь с чистой совестью выпинуть его из трамвая обратно в его мир! Перетащи его к люку в центре");
        phrasesByFrames.Add(7, "Отлично! Теперь быстро тапай по нему несколько раз, чтобы выпинуть! Да смотри, чтобы не убежал!");
        phrasesByFrames.Add(8, "Это было просто, правда? Старайся, чтобы до следующей остановки доехало как можно меньше зайцев, иначе честным путешественникам будет труднее добраться туда, куда они хотят");
        phrasesByFrames.Add(9, "Иногда двери заклинивает. Нужно успеть вытащить пассажира, иначе ему несдобровать!");
        phrasesByFrames.Add(10, "Отлично, теперь можно двигаться дальше");
        phrasesByFrames.Add(11, "Ну вот, в следующий раз следи за дверями внимательнее");
        phrasesByFrames.Add(12, "Некоторые пассажиры терпеть не могут тех, кто оказался рядом с ними в этом трамвае. Они могут даже напасть на кондуктора! Если обидчик без билета, смело выпинывай его. Но, если нападает тот, у кого есть билет - будь осторожен. Ты в ответе за честных пассажиров");
        phrasesByFrames.Add(13, "Если этот счётчик не доехавших дойдёт до нуля, маршрут придётся начинать заново. Старайся, чтобы те, кто купил билет, добрались до своей цели! Рассаживай дерущихся подальше друг от друга");
        phrasesByFrames.Add(14, "Да, здесь может быть опасно даже для тебя. Старайся, чтобы тебя не задели");
        phrasesByFrames.Add(15, "Посмотри, что там в сундучке");
        phrasesByFrames.Add(16, "Ты можешь купить этот и другие бонусы перед маршрутом в магазине");
        phrasesByFrames.Add(17, "Попробуй усадить пассажира сюда - если на сидениях несколько одинаковых пассажиров, ехать сразу становится веселее!");
        phrasesByFrames.Add(18, "Ну что ж, пора расставаться. Я верю, ты справишься. А мне и здесь хорошо. Ярких миров!");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPause)
            if (Time.timeScale > 0.02f)
                Time.timeScale -= 0.01f;
            else
                Time.timeScale = 0;
    }

    public void SceneChange()
    {
        if (!_isTraining)
        {
            if (_currentNum < 4)
            {
                if (!_sceneChanging)
                {
                    _sceneChanging = true;
                    _text.gameObject.SetActive(true);
                    _text.text = phrasesByFrames[_currentNum];
                    _currentNum++;
                }
                else
                {
                    _sceneChanging = false;
                    _text.gameObject.SetActive(false);
                    _image.sprite = Resources.Load<Sprite>(@"Sprites/training/" + _currentNum);
                }
            }
            else
            {
                _text.gameObject.SetActive(false);
                _image.gameObject.SetActive(false);
                _audio.gameObject.SetActive(true);
                _interface.gameObject.SetActive(true);
                _all.gameObject.SetActive(true);
                _isTraining = true;
                Time.timeScale = 1;
                _cloudText.text = phrasesByFrames[_currentNum];
            }
        }
        else
        {
            if (_isPause)
            {
                _isPause = false;
            }
        }
    }
}
