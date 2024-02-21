using UnityEngine;
using System.Collections.Generic;

public class HandTransformPredictor : MonoBehaviour
{
    List<Vector3> positionHistory = new List<Vector3>();
    List<Quaternion> rotationHistory = new List<Quaternion>();
    [SerializeField] GameObject targetObj;
    [SerializeField] GameObject sourceObj;

    private void Update()
    {
        // 過去0.1秒間の座標と角度の履歴を取得
        Vector3 predictedPosition = PredictPosition(0.1f);
        Quaternion predictedRotation = PredictRotation(0.1f);

        targetObj.transform.position = predictedPosition;
        targetObj.transform.rotation = predictedRotation;

    }

    private void FixedUpdate()
    {
        // 座標と角度の履歴を更新
        positionHistory.Add(sourceObj.transform.position);
        rotationHistory.Add(sourceObj.transform.rotation);
    }

    private Vector3 PredictPosition(float time)
    {
        if (positionHistory.Count < 2)
            return sourceObj.transform.position;

        Vector3 currentPosition = positionHistory[positionHistory.Count - 1];
        Vector3 previousPosition = positionHistory[positionHistory.Count - 2];

        // 直近の変位から速度を算出し、予測された位置を計算
        Vector3 velocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;
        return currentPosition + velocity * time;
    }

    private Quaternion PredictRotation(float time)
    {
        if (rotationHistory.Count < 2)
            return sourceObj.transform.rotation;

        Quaternion currentRotation = rotationHistory[rotationHistory.Count - 1];
        Quaternion previousRotation = rotationHistory[rotationHistory.Count - 2];

        // 直近の回転から角速度を算出し、予測された角度を計算
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);
        float angle = 0f;
        Vector3 axis = Vector3.zero;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = axis * Mathf.Deg2Rad * angle / Time.fixedDeltaTime;

        // 時間をかけて角度を予測
        Quaternion predictedRotation = currentRotation;
        predictedRotation.eulerAngles += angularVelocity * time;
        return predictedRotation;
    }
}
