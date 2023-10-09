using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Match3D
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _pool;

        [SerializeField]
        private Transform _matchingPool;

        [SerializeField]
        private GameObject[] _itemPrefabs;

        [SerializeField]
        private int _coupleCount;

        [SerializeField]
        private int _matchCount;

        [SerializeField] private TextMeshProUGUI MatchText;

        [SerializeField]
        private Camera _gameCamera;

        public Countdown countdown;
        public UIManager uıManager;
        

        public Camera GameCamera
        {
            get { return _gameCamera; }
        }
        

        public event Action<SelectableItem> OnItemEntersBox;
        public event Action<SelectableItem> OnItemLeavesBox;

        [SerializeField]
        private List<SelectableItem> _itemsInsideBox;


        private void Start()
        {
            Time.timeScale = 1;
            if (_gameCamera == null)
            {
                _gameCamera = Camera.main;
            }

            _itemsInsideBox = new List<SelectableItem>();
            

            FillPool();

            OnItemEntersBox += ItemEntersBox;
            OnItemLeavesBox += ItemLeavesBox;
        }

        private void OnDestroy()
        {
            OnItemEntersBox -= ItemEntersBox;
            OnItemLeavesBox -= ItemLeavesBox;
        }

        private void Update()
        {
            GameOver();
            Win();
        }

        public void GameOver()
        {
            if (_matchCount<_coupleCount && countdown.remainingTime < 1 )
            {
                uıManager.GameOver();
                Time.timeScale = 0;
            }
        }

        public void Win()
        {
            if (_matchCount== _coupleCount)
            {
                uıManager.Win();
                Time.timeScale = 0;
            }
        }
        private void FillPool()
        {
            for (int i = 0; i < _coupleCount; i++)
            {
                GameObject go1 = Instantiate(_itemPrefabs[i % _itemPrefabs.Length], RandomPositionOverPool(), Quaternion.identity);
                go1.transform.SetParent(transform);
                go1.GetComponent<SelectableItem>().Init(this);

                GameObject go2 = Instantiate(_itemPrefabs[i % _itemPrefabs.Length], RandomPositionOverPool(), Quaternion.identity);
                go2.transform.SetParent(transform);
                go2.GetComponent<SelectableItem>().Init(this);
            }
        }


        private Vector3 RandomPositionOverPool()
        {
            Vector3 position = _pool.position;

            position += new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), UnityEngine.Random.Range(3f, 6f), UnityEngine.Random.Range(-4f, 6.5f));

            return position;
        }


        public void PlaceItemInsideBox(SelectableItem item)
        {
            OnItemEntersBox?.Invoke(item);

        }

        public void RemoveItemFromBox(SelectableItem item)
        {
            OnItemLeavesBox?.Invoke(item);
        }

        private void ItemLeavesBox(SelectableItem item)
        {
            if (_itemsInsideBox.Contains(item))
                _itemsInsideBox.Remove(item);

            if (_itemsInsideBox.Count == 2)
            {
                CheckIfItemsMatch();
            }
        }

        private void ItemEntersBox(SelectableItem item)
        {
            _itemsInsideBox.Add(item);
            if (_itemsInsideBox.Count == 2)
            {
                CheckIfItemsMatch();
            }
        }

        private void CheckIfItemsMatch()
        {
            if (_itemsInsideBox[0].type == _itemsInsideBox[1].type)
            {
                foreach (SelectableItem item in _itemsInsideBox)
                    Destroy(item.gameObject);

                _itemsInsideBox.Clear();

                _matchCount += 1;
                MatchText.text = _matchCount.ToString();
            }

        }
    }

}