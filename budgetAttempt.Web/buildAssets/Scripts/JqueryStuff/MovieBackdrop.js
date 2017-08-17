(function($) {
	$.fn.backtube = function(options) {
		var totalIndex = this.length - 1;
		var elementOnBody = false;
		return this.each(function(index) {
			// Insert Youtube iFrame API
			if (typeof YT != 'object') {
				var tag = document.createElement('script');
				tag.src = 'https://www.youtube.com/iframe_api';
				var firstScriptTag = document.getElementsByTagName('script')[0];
				firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
			}

			// Run only on the last element
			if (totalIndex === index) {
				// If only a string is set as options, use it as id for the Youtube Movie
				if (typeof options === 'string') {
					options = {
						id: options
					};
				}

				if (document.body == this) {
					elementOnBody = true;
					$(this).addClass('backtube-body');
				}

				// Merge options
				options = $.extend({}, $.backtube.defaults, options);

				// Save elements in variables
				var _this = this;
				var $muteButton = $(options.selector.mute).not(options.selector.mute + '-init');
				var $container = $(options.template);
				var $playContainer = $container.filter(options.selector.container);

				var playLength = false;

				// Fade in actual container
				var showContainer = function() {
					setTimeout(function() {
						$playContainer.removeClass(options.classes.fadeIn);
					}, options.time.fadeIn);
				};

				// Reset container
				var resetContainer = function(fadeIn) {
					$.backtube.active = false;
					$(document).data('backtube-playing', false);
					$(document.body).removeClass(options.classes.playing);
					$playContainer.toggleClass(options.classes.fadeIn, fadeIn);
					setTimeout(function() {
						$container.remove();
					}, options.time.fadeIn);
				};

				var removeContainer = function() {
					$(document).data('backtube-playing', false);
					$(document.body).removeClass(options.classes.playing);
					$container.remove();
				};

				var fadeOutContainer = function() {
					$playContainer.addClass(options.classes.fadeOut);
					setTimeout(function() {
						removeContainer();
						// Callback onEnd
						if (typeof options.onEnd === 'function') {
							options.onEnd();
						}
					}, 1100);
				};

				// Player state change (e.g play / pause / stop)
				var onPlayerStateChange = function(state) {
					if (state.data === YT.PlayerState.PLAYING) {
						setTimeout(function() {
                            $.backtube.player.mute();
							$(document).data('playing',true);
							$(document.body).addClass(options.classes.playing).removeClass(options.classes.init);
							showContainer();
						}, 200);

						// Stop Playing after a defined time
						if (typeof options.playLength === 'number') {
							playLength = setTimeout(fadeOutContainer, options.playLength * 1000);
						}

						// Callback onStart
						if (typeof options.onStart === 'function') {
							options.onStart();
						}
					} else if (state.data === YT.PlayerState.PAUSED) {
						fadeOutContainer();
						// Callback onEnd
						if (typeof options.onEnd === 'function') {
							options.onEnd();
						}
					} else if (state.data === YT.PlayerState.ENDED) {
						// Loop video
						if (options.loop) {
							$.backtube.player.seekTo(0);
						} else {
							resetContainer(options.fadeIn);
							// Callback onEnd
							if (typeof options.onEnd === 'function') {
								options.onEnd();
							}
						}
					}
				};

				// When player is ready
				var onPlayerReady = function(element) {
					if ($(document).data('backtube-mute')) {
						element.target.mute();
					}
					if (typeof options.onReady === 'function') {
						options.onReady();
					}
					if (playLength) {
						clearTimeout(playLength);
					}
					$(document.body).addClass(options.classes.init);
					setTimeout(function() {
						$(document.body).addClass(options.classes.button);
					}, 4000);
				};

				if (!$(_this).hasClass(options.classes.wrapper)) {
					$(_this).addClass(options.classes.wrapper);
				}
				// Remove container at start
				removeContainer(options.fadeIn);

				// Set class @ mute button
				$muteButton
				.addClass(options.selector.mute + '-init')
				.toggleClass(options.classes.mute, $(document).data('backtube-mute') || options.mute);

				// Mute Button
				$muteButton.on('touchend click', function(event) {
					event.preventDefault();
					if ($(document).data('backtube-mute')) {
						$(document).data('backtube-mute', false);
						try {
							$.backtube.player.unMute();
						} catch (e) {}
					} else {
						$(document).data('mute', true);
						try {
							$.backtube.player.mute();
						} catch (e) {}
					}

					// Bei Mausklick die Klasse erst nach verlassen des Buttons austauschen
					if (event.type === 'click') {
						$(this).one('mouseleave', function() {
							$muteButton.toggleClass('mute', $(document).data('mute'));
						});
					} else {
						$muteButton.toggleClass('mute', $(document).data('mute'));
					}

				});

				// The Youtube API
				window.onYouTubeIframeAPIReady = function() {
					$.backtube.active = true;
					if (elementOnBody) {
						$container.prependTo(_this);
					} else {
						$container.insertBefore(_this);
					}
					$.backtube.player = new YT.Player('backtube-player', {
						width: 250,
						height: 250,
						videoId: options.id,
						playerVars: {
							controls       : 0, // Controller abstellen
							showinfo       : 0, // Infos ausblenden
							disablekb      : 1, // Tastatur deaktivieren
							//enablejsapi    : 1, // JS API aktivieren
							iv_load_policy : 3, // Video-Anmerkungen abstellen
							modestbranding : 1, // Kein Youtube Logo
							autoplay       : options.autoplay ? 1 : 0, // Autoplay
							wmode          : 'transparent'
						},
						events: {
							'onReady': onPlayerReady,
							'onStateChange': onPlayerStateChange
						}
					});
				};

				setTimeout(function() {
					if (typeof YT === 'object') {
						if (typeof YT.Player === 'function' && !$.backtube.active) {
							// Falls YouTube Api ready ist, aber die Funktion 'onYouTubeIframeAPIReady' noch nicht aufgerufen wurde
							onYouTubeIframeAPIReady();
						}
					}
				}, 50);
			}
		});
	};

	$.backtube = {
		defaults: {
			template: '<div class="backtube-container backtube-fade-in">' +
			'<div id="backtube-player"></div></div><div class="backtube-shield">' +
			'</div>',
			classes: {
				wrapper: 'backtube-wrapper',
				playing: 'backtube-playing',
				init: 'backtube-init',
				fadeIn: 'backtube-fade-in',
				fadeOut: 'backtube-fade-out',
				mute: 'backtube-mute',
				button: 'backtube-playbutton'
			},
			selector: {
				mute: '.backtube-mute',
				container: '.backtube-container'
			},
			time: {
				fadeIn: 800
			},
			id: false,
			zIndex: 10,
			loop: false,
			mute: true,
			autoplay: true,
			fadeIn: true,
			onReady: null,
			onStart: null,
			onEnd: null,
			playLength: null
		},
		active: false,
		player: null,
		id: null,
		stop: function() {
			if (typeof $.backtube.player === 'object') {
				$.backtube.player.pauseVideo();
			}
		}
	};
})(jQuery);

(function($) {
	$(document.body).backtube({
		id: 'NZlfxWMr7nc',
		loop: true
	});
})(jQuery);
