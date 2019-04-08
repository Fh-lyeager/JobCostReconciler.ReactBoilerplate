// Source from auth0/auth0.js library
export default class IFrameHandler {
  constructor(options) {
    this.url = options.url;
    this.callback = options.callback;
    this.timeout = options.timeout || 60 * 1000;
    this.timeoutCallback = options.timeoutCallback || null;
    this.eventListenerType = "load";
  }

  init() {
    const _this = this;

    this.iframe = window.document.createElement("iframe");
    this.iframe.style.display = "none";
    this.iframe.src = this.url;

    // Workaround to avoid using bind that does not work in IE8
    this.proxyEventListener = function(e) {
      _this.eventListener(e, this.contentWindow.location);
    };

    this.eventSourceObject = this.iframe;

    this.eventSourceObject.addEventListener(
      this.eventListenerType,
      this.proxyEventListener,
      false
    );

    window.document.body.appendChild(this.iframe);

    this.timeoutHandle = setTimeout(function() {
      _this.timeoutHandler();
    }, this.timeout);
  }

  eventListener(event, location) {
    const eventData = {
      event: event,
      sourceObject: this.eventSourceObject,
      location
    };

    this.callback(eventData);
    this.destroy();
  }

  timeoutHandler() {
    this.destroy();
    if (this.timeoutCallback) {
      this.timeoutCallback();
    }
  }

  destroy() {
    const _this = this;

    clearTimeout(this.timeoutHandle);

    setTimeout(() => {
      _this.eventSourceObject.removeEventListener(
        _this.eventListenerType,
        _this.proxyEventListener,
        false
      );
      window.document.body.removeChild(_this.iframe);
    }, 0);
  }
}
