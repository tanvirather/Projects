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
      return true;
    }
    return false;
  }

  async update(model) {
    return await this.#apiClient.put(this.#url, model, { 'Authorization': `Bearer ${this.#storage.token}` });
  }
}
