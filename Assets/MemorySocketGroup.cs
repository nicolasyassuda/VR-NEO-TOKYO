using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public class SocketGroup
{
    public string groupName;
    public XRSocketInteractor[] sockets; // 6 sockets per group
}

public class MemorySocketGroup : MonoBehaviour
{
    [Header("Groups (3 groups of 6 sockets each)")]
    [SerializeField] private SocketGroup[] groups;

    [Header("Events")]
    public UnityEvent onAllMatch;     // fires when all 3 groups share the same value
    public UnityEvent onMatchBroken;  // fires when match is broken

    private bool _wasMatching = false;

    void OnEnable()
    {
        foreach (var g in groups)
            foreach (var s in g.sockets)
            {
                s.selectEntered.AddListener(OnSocketChanged);
                s.selectExited.AddListener(OnSocketChanged);
            }
    }

    void OnDisable()
    {
        foreach (var g in groups)
            foreach (var s in g.sockets)
            {
                s.selectEntered.RemoveListener(OnSocketChanged);
                s.selectExited.RemoveListener(OnSocketChanged);
            }
    }

    private void OnSocketChanged(SelectEnterEventArgs args) => CheckAllGroups();
    private void OnSocketChanged(SelectExitEventArgs args) => CheckAllGroups();

    private void CheckAllGroups()
    {
        bool matching = AllGroupsMatch(out int matchedValue);

        if (matching && !_wasMatching)
        {
            Debug.Log($"[MemorySocket] All groups match! Value: {matchedValue}");
            _wasMatching = true;
            onAllMatch.Invoke();
        }
        else if (!matching && _wasMatching)
        {
            Debug.Log("[MemorySocket] Match broken.");
            _wasMatching = false;
            onMatchBroken.Invoke();
        }
    }

    private bool AllGroupsMatch(out int matchedValue)
    {
        matchedValue = -1;
        int globalSum = int.MinValue;

        foreach (var group in groups)
        {
            int groupSum = GetGroupSum(group, out bool hasAny);

            // Group must have at least one filled socket
            if (!hasAny) return false;

            if (globalSum == int.MinValue)
                globalSum = groupSum;       // first group sets the target sum
            else if (groupSum != globalSum)
                return false;               // sums don't match
        }

        matchedValue = globalSum;
        return true;
    }

    // Sums all filled socket values in a group, ignoring empty ones
    private int GetGroupSum(SocketGroup group, out bool hasAny)
    {
        hasAny = false;
        int sum = 0;

        foreach (var socket in group.sockets)
        {
            if (!socket.hasSelection) continue;

            var mem = socket.firstInteractableSelected.transform.GetComponent<MemoryValue>();
            if (mem == null) continue;

            sum += mem.memoryValue;
            hasAny = true;
        }

        return sum;
    }

    // Public helpers
    public bool IsMatching() => _wasMatching;

    // Get the current sum of a group (-1 if empty)
    public int GetGroupSum(int groupIndex)
    {
        if (groupIndex < 0 || groupIndex >= groups.Length) return -1;
        int sum = GetGroupSum(groups[groupIndex], out bool hasAny);
        return hasAny ? sum : -1;
    }

    // How many groups have at least one filled socket
    public int ConsistentGroupCount()
    {
        int count = 0;
        foreach (var g in groups)
        {
            GetGroupSum(g, out bool hasAny);
            if (hasAny) count++;
        }
        return count;
    }
}