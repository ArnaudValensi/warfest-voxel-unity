using UnityEngine;
using System.Collections;

// TODO: use hashes also instead of animation names.
public class SpriteAnimator : MonoBehaviour {

	[System.Serializable]
	public class AnimationTrigger {
		public int frame;
		public string name;
	}

	[System.Serializable]
	public class Animation {
		public string name;
		public int fps;
		public Sprite[] frames;

		public AnimationTrigger[] triggers;
	}

	public SpriteRenderer spriteRenderer;
	public Animation[] animations;

	public bool playing { get; private set; }

	public Animation currentAnimation { get; private set; }

	public int currentFrame { get; private set; }

	public bool loop { get; private set; }

	public string playAnimationOnStart;

	void Awake() {
		if (!spriteRenderer)
			spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnEnable() {
		if (playAnimationOnStart != "")
			Play(playAnimationOnStart);
	}

	void OnDisable() {
		playing = false;
		currentAnimation = null;
	}

	public void Play(string name, bool loop = true, int startFrame = 0) {
		Animation animation = GetAnimation(name);
		if (animation != null) {
			if (animation != currentAnimation) {
				ForcePlay(name, loop, startFrame);
			}
		} else {
			Debug.LogWarning("could not find animation: " + name);
		}
	}

	public void ForcePlay(string name, bool loop = true, int startFrame = 0) {
		Animation animation = GetAnimation(name);
		if (animation != null) {
			this.loop = loop;
			currentAnimation = animation;
			playing = true;
			currentFrame = startFrame;
			spriteRenderer.sprite = animation.frames[currentFrame];
			StopAllCoroutines();
			StartCoroutine(PlayAnimation(currentAnimation));
		}
	}

	public void SlipPlay(string name, int wantFrame, params string[] otherNames) {
		for (int i = 0; i < otherNames.Length; i++) {
			if (currentAnimation != null && currentAnimation.name == otherNames[i]) {
				Play(name, true, currentFrame);
				break;
			}
		}
		Play(name, true, wantFrame);
	}

	public bool IsPlaying(string name) {
		return (currentAnimation != null && currentAnimation.name == name);
	}

	public void Stop() {
		playing = false;
		currentAnimation = null;
		StopAllCoroutines();
	}

	public Animation GetAnimation(string name) {
		foreach (Animation animation in animations) {
			if (animation.name == name) {
				return animation;
			}
		}
		return null;
	}

	IEnumerator PlayAnimation(Animation animation) {
		float timer = 0f;
		float delay = 1f / (float)animation.fps;
		while (loop || currentFrame < animation.frames.Length - 1) {

			while (timer < delay) {
				timer += Time.deltaTime;
				yield return 0f;
			}
			while (timer > delay) {
				timer -= delay;
				NextFrame(animation);
			}

			spriteRenderer.sprite = animation.frames[currentFrame];
		}

		currentAnimation = null;
	}

	void NextFrame(Animation animation) {
		currentFrame++;
		foreach (AnimationTrigger animationTrigger in currentAnimation.triggers) {
			if (animationTrigger.frame == currentFrame) {
				gameObject.SendMessageUpwards(animationTrigger.name);
			}
		}

		if (currentFrame >= animation.frames.Length) {
			if (loop)
				currentFrame = 0;
			else
				currentFrame = animation.frames.Length - 1;
		}
	}

	public int GetFacing() {
		return (int)Mathf.Sign(spriteRenderer.transform.localScale.x);
	}

	public void FlipTo(float dir) {
		if (dir < 0f)
			spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
		else
			spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void FlipTo(Vector3 position) {
		float diff = position.x - transform.position.x;
		if (diff < 0f)
			spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
		else
			spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
	}

}