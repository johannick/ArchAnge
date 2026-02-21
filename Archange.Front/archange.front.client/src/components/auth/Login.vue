<template>
  <div>
    <div class="mb-3">
      <div class="input-group">
        <div class="input-group-text">Email address</div>
        <input type="email" class="form-control" placeholder="address@domain.com" :value="{{email.value}}" />
      </div>
    </div>
    <div class="mb-3">
      <div class="input-group">
        <div class="input-group-text">Password</div>
        <input type="password" class="form-control" :value="{{password.value}}" />
      </div>
    </div>

    <div class="mb-3 form-check">
      <input type="checkbox" class="form-check-input" id="rememberMe" :checked="{{rememberMe.value}}">
      <label class="form-check-label" for="rememberMe">Remember me</label>
    </div>

    <div class="mb-3 btn-group" role="group" aria-label="Login or register">
      <input type="radio" class="btn-check" name="Auth" id="Login" autocomplete="off" @click="login.value = true" :checked="{{login.value === true}}">
      <label class="btn btn-outline-primary" for="Login">Login</label>

      <input type="radio" class="btn-check" name="Auth" id="Register" autocomplete="off" @click="login.value = false" :checked="{{login.value === false}}">
      <label class="btn btn-outline-primary" for="Register">Register</label>
    </div>

    <div v-if="login.value">

      <div class="input-group mb-3">
        <div class="input-group-text">Provider</div>
        <select class="form-select" :value="{{Providers}}">
          <option v-for="provider in Providers" :key="provider" :value="provider">{{provider}}</option>
        </select>
      </div>
    </div>

    <div class="mb-3" v-if="!login.value">
      <div class="mb-3">
        <div class="input-group">
          <div class="input-group-text">Confirm Password</div>
          <input type="password" class="form-control" :value="{{confirmPassword.value}}">
        </div>
      </div>

      <div class="mb-3" v-if="!login.value">
        <div class="input-group">
          <input type="radio" class="btn-check" name="Auth" id="Unknown" autocomplete="off" @click="profile.gender = Gender.Unknown" :checked="{{profile.gender === Gender.Unknown}}">
          <label class="btn btn-outline-primary" for="Unknown">Unknown</label>

          <input type="radio" class="btn-check" name="Auth" id="Male" autocomplete="off" @click="profile.gender = Gender.Male" :checked="{{profile.gender === Gender.Male}}">
          <label class="btn btn-outline-primary" for="Male">Male</label>

          <input type="radio" class="btn-check" name="Auth" id="Female" autocomplete="off" @click="profile.gender = Gender.Female" :checked="{{profile.gender === Gender.Female}}">
          <label class="btn btn-outline-primary" for="Male">Female</label>
        </div>
      </div>
    </div>

    <div class="mb-3">
      <button type="submit" class="btn btn-primary" :disabled="{{loading.value}}">Login</button>
      <button type="button" class="btn btn-secondary" @click="loading.value = true">Loading...</button>
    </div>
      

    </div>
</template>

<script lang="ts">

  import ref from 'vue';
  import ProfileModel from './api/apitrest.ts'

  const login = ref(true);
  const email = ref('');
  const password = ref('');
  const confirmPassword = ref('');
  const profile = new ProfileModel();
  const rememberMe = ref(false);
  const loading = ref(false);

  const Providers = ['Google', 'Microsoft', 'Apple', 'Facebook', 'Twitter', 'Instagram'];

</script>
