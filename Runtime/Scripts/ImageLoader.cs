using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Image;
using VRC.Udon.Common.Interfaces;

public class ImageLoader : UdonSharpBehaviour
{
    private VRCImageDownloader _imageDownloader;
    private IUdonEventReceiver _udonEventReceiver;

    [SerializeField, Tooltip("URL of the image to load")]
    private VRCUrlInputField _imageUrl;

    [SerializeField, Tooltip("Text to show error messages on.")]
    private TMPro.TextMeshProUGUI _errorText;

    [SerializeField, Tooltip("Renderer to show downloaded images on.")]
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        // To receive Image and String loading events, 'this' is casted to the type needed
        _udonEventReceiver = (IUdonEventReceiver)this;
        _imageDownloader = new VRCImageDownloader();
    }

    public void LoadImage()
    {
        var res = _imageDownloader.DownloadImage(_imageUrl.GetUrl(), _renderer.material, _udonEventReceiver);
        if (res.State == VRCImageDownloadState.Error && _errorText)
            _errorText.text = $"Failed to start downloading image: {res.ErrorMessage}";
        if (res.State == VRCImageDownloadState.Complete)
        {
            var widthRatio = res.Result.width / (float)res.Result.height;
            var heightRatio = res.Result.height / (float)res.Result.width;
        }
    }

    private void OnDestroy()
    {
        _imageDownloader.Dispose();
    }
}
