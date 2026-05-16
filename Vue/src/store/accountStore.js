export class AccountStore {
  #apiClient;
  #storage;
  #url = "account"

  constructor(apiClient, storage) {
    this.#apiClient = apiClient
    this.#storage = storage
  }

  async register(model) {
    return await this.#apiClient.post(this.#url + "/register", model);
  }

  async get(id) {
    let url = this.#url;
    if (id) {
      url += `/${id}`;
    }
    return await this.#apiClient.get(url, { 'Authorization': `Bearer ${this.#storage.token}` });
  }

  async verifyEmail(email, token) {
    return await this.#apiClient.post(this.#url + "/verifyemail", { email, token });
  }

  async login(model) {
    const response = await this.#apiClient.post(this.#url + "/login", model);
    if (response.token) {
      this.#storage.token = response.token;
      return { success: true };
    }
    if (response.requiresTwoFactor) {
      return { requiresTwoFactor: true, userId: response.userId };
    }
    return { success: false };
  }

  async loginTwoFactor(userId, code) {
    const response = await this.#apiClient.post(this.#url + "/loginTwoFactor", { userId, code });
    if (response.token) {
      this.#storage.token = response.token;
      return true;
    }
    return false;
  }

  async getEnableTwoFactor(id) {
    return await this.#apiClient.get(this.#url + `/enableTwoFactor/${id}`, { 'Authorization': `Bearer ${this.#storage.token}` });
  }

  async enableTwoFactor(userId, code) {
    return await this.#apiClient.post(this.#url + "/enableTwoFactor", { userId, code }, { 'Authorization': `Bearer ${this.#storage.token}` });
  }

  async update(model) {
    return await this.#apiClient.put(this.#url, model, { 'Authorization': `Bearer ${this.#storage.token}` });
  }

  async verifyPhoneNumber(id) {
    return await this.#apiClient.post(this.#url + `/verifyphone/${id}`, {}, { 'Authorization': `Bearer ${this.#storage.token}` });
  }

  async confirmPhoneNumber(id, token) {
    return await this.#apiClient.post(this.#url + "/confirmphone", { id, token }, { 'Authorization': `Bearer ${this.#storage.token}` });
  }
}
