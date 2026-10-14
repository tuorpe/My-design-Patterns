using UnityEngine;

namespace Observer
{
    public class Achievement
    {
        public readonly string name;
        public readonly string description;
        private readonly int _valueLimit;
        private bool _isAchieved;
        private int _currentValue;
        private int _value;
    
        public Achievement(string name, string description, int valueLimit)
        {
            this.name = name;
            this.description = description;
            this._valueLimit = valueLimit;
        }
    
        public Achievement Achieve()
        {
            _value++;
            if (_value < _valueLimit) 
                return null;
            if(_isAchieved)
                return null;
            _isAchieved = true;
            Debug.Log($"Achievement unlocked: {name}\n Description: {description}");
            return this;
        }
    
    }
}
