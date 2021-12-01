using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;

public class LevelController : MonoBehaviour
{

    [SerializeField] private List<GameObject> _leveller = new List<GameObject>();

    private int _levelNumarasi;

    private GameObject güncelLevel;

    private int _levelNumber;

    private int _toplamLevelSayisi;

    void Start()
    {
        if (güncelLevel)
        {
            Destroy(güncelLevel);
        }
        else
        {

        }
       // PlayerPrefs.SetInt("LevelNumarası", 0);

        _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
        _levelNumber = PlayerPrefs.GetInt("LevelNumber");
        _toplamLevelSayisi = _leveller.Count - 1;

        if (_levelNumber < _toplamLevelSayisi)
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
            güncelLevel  = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
            Elephant.LevelStarted(_levelNumber);
        }
        else
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");

            güncelLevel = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
            Elephant.LevelStarted(_levelNumber);
        }
       


    }

    
    public void LevelDegistir()
    {
        Destroy(güncelLevel);
        _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
        _levelNumber = PlayerPrefs.GetInt("LevelNumber");
        _toplamLevelSayisi = _leveller.Count - 1;
        Elephant.LevelCompleted(_levelNumber);

        if (_levelNumber < _toplamLevelSayisi)
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
            _levelNumarasi += 1;
            _levelNumber++;

            güncelLevel = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
            PlayerPrefs.SetInt("LevelNumarasi", _levelNumarasi);
            PlayerPrefs.SetInt("LevelNumber", _levelNumber);
            Elephant.LevelStarted(_levelNumber);
        }
        else
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
            int _geciciLevelNumarasi = _levelNumarasi;
            
            _levelNumarasi = Random.Range(0, _toplamLevelSayisi);

            if (_levelNumarasi == _geciciLevelNumarasi)
            {
                LevelDegistir();
            }
            else
            {
                _levelNumber++;
                güncelLevel = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
                PlayerPrefs.SetInt("LevelNumarasi", _levelNumarasi);
                PlayerPrefs.SetInt("LevelNumber", _levelNumber);
                Elephant.LevelStarted(_levelNumber);
            }
        

            

        }

        
    }

    public void LevelRestart()
    {
        Destroy(güncelLevel);
        _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
        _toplamLevelSayisi = _leveller.Count - 1;
        Elephant.LevelFailed(_levelNumber);

        if (_levelNumber < _toplamLevelSayisi)
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");
            güncelLevel = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
            Elephant.LevelStarted(_levelNumber);
        }
        else
        {
            _levelNumarasi = PlayerPrefs.GetInt("LevelNumarasi");

            güncelLevel = Instantiate(_leveller[_levelNumarasi], new Vector3(0, 0, 0), Quaternion.identity);
            Elephant.LevelStarted(_levelNumber);
        }
    }
}
