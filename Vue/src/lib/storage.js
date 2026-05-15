export class Storage {
  #tokenKey = "token";
  get token() { return localStorage.getItem(this.#tokenKey); }
  set token(value) { this.#set("token", value); }

  get userId() {
    const token = this.token;
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.nameid || payload.sub;
    } catch (e) {
      return null;
    }
  }

  #set(key, value) {
    value ? localStorage.setItem(key, value) : localStorage.removeItem(key);
  }

  clear() {
    localStorage.clear();
    sessionStorage.clear();
  }
}

export const storage = new Storage();
