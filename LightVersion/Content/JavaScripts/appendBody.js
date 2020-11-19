var scriptEl = document.createElement('script');
scriptEl.text = `{{script}}`;
scriptEl.type = "text/javascript";
document.getElementsByTagName("body")[0].appendChild(scriptEl);