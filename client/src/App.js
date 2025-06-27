import { useState } from "react";
import "./App.css";

function App() {
  const [todos, setTodos] = useState([]);

  const createTodo = async () => {
    try {
      const res = await fetch(`http://localhost:5128/todos`)
      if (!res.ok) {
        throw new Error(res)
      }
      const json = await res.json()
      console.log(json)
    } catch (error) {
      console.error(error)
    }
  };

  return (
    <div>
      <aside className="sidebar">
        <div className="sidenav">
          <button onClick={createTodo}>Create Todo</button>
        </div>
      </aside>
      <div className="main">
        <h2>
          My Todos
        </h2>
      </div>
    </div>
  );
}

export default App;
