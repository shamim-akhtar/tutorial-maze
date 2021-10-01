using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraManipulator2D : MonoBehaviour
{
  // Set the min and the max size for this 
  // camera so that we can fix the limits
  // of our zoom in and zoom out.
  [SerializeField]
  float CameraSizeMin = 1.0f;
  [SerializeField]
  float CameraSizeMax = 10.0f;

  // The slider for zoom-in and zoom-out
  [SerializeField]
  public Slider SliderZoom;

  [SerializeField]
  FixedButton BtnZoomIn;
  [SerializeField]
  FixedButton BtnZoomOut;

  // A property to allow/disallow 
  // panning of camera.
  // You can set panning to be false
  // if you want to disable panning.
  // One exmaple could be when you are
  // dragung or interacting with a game object 
  // then you can disable camera panning.
  public static bool IsCameraPanning
  {
    get;
    set;
  } = true;

  // Some variables needed for dragging our 
  // camera to creae the pan control
  private Vector3 mDragPos;
  private Vector3 mOriginalPosition;
  private bool mDragging = false;

  // The zoom factor
  private float mZoomFactor = 0.0f;

  // Save a reference to the Camera.main
  public Camera mCamera;

  void Start()
  {
    if(CameraSizeMax < CameraSizeMin)
    {
      float tmp = CameraSizeMax;
      CameraSizeMax = CameraSizeMin;
      CameraSizeMin = tmp;
    }

    if(CameraSizeMax - CameraSizeMin < 0.01f)
    {
      CameraSizeMax += 0.1f;
    }

    SetCamera(mCamera);
  }

  public void SetCamera(Camera camera)
  {
    mCamera = camera;
    mOriginalPosition = mCamera.transform.position;

    // For this demo, we simple take the current camera
    // and calculate the zoom factor.
    // Alternately, you may want to set the zoom factor
    // in other ways. For example, randomize the 
    // zoom factory between 0 and 1.

    mZoomFactor = 
      (CameraSizeMax - mCamera.orthographicSize) / 
      (CameraSizeMax - CameraSizeMin);

    if (SliderZoom)
    {
      SliderZoom.value = mZoomFactor;
    }
  }

  void Update()
  {
    if (BtnZoomIn && BtnZoomIn.Pressed)
    {
      ZoomIn();
    }
    if (BtnZoomOut && BtnZoomOut.Pressed)
    {
      ZoomOut();
    }

    // Camera panning is disabled when a tile is selected.
    if (!IsCameraPanning)
    {
      mDragging = false;
      return;
    }

    // We also check if the pointer is not on UI item
    // or is disabled.
    if (EventSystem.current.IsPointerOverGameObject() || 
      enabled == false)
    {
      //mDragging = false;
      return;
    }

    // Save the position in worldspace.
    if (Input.GetMouseButtonDown(0))
    {
      mDragPos = mCamera.ScreenToWorldPoint(
        Input.mousePosition);
      mDragging = true;
    }

    if (Input.GetMouseButton(0) && mDragging)
    {
      Vector3 diff = mDragPos - 
        mCamera.ScreenToWorldPoint(
          Input.mousePosition);
      diff.z = 0.0f;
      mCamera.transform.position += diff;
    }
    if (Input.GetMouseButtonUp(0))
    {
      mDragging = false;
    }
  }

  public void ResetCameraView()
  {
    mCamera.transform.position = mOriginalPosition;
    mCamera.orthographicSize = CameraSizeMax;
    mZoomFactor = 0.0f;
    if (SliderZoom)
    {
      SliderZoom.value = 0.0f;
    }
  }

  public void OnSliderValueChanged()
  {
    Zoom(SliderZoom.value);
  }

  public void Zoom(float value)
  {

    mZoomFactor = value;
    // clamp the value between 0 and 1.
    mZoomFactor = Mathf.Clamp01(mZoomFactor);

    if(SliderZoom)
    {
      // if the slider is valied
      // set the value to the slider.
      SliderZoom.value = mZoomFactor;
    }

    if (mCamera == null)
      return;
    // set the camera size
    mCamera.orthographicSize = CameraSizeMax -
        mZoomFactor * 
        (CameraSizeMax - CameraSizeMin);
  }

  public void ZoomIn()
  {
    // For this demo we hardcode 
    // the increment. You should
    // avoid hardcoding the value.
    Zoom(mZoomFactor + (CameraSizeMax - CameraSizeMin) * 0.0001f);
  }

  public void ZoomOut()
  {
    Zoom(mZoomFactor - (CameraSizeMax - CameraSizeMin) * 0.0001f);
  }
}
