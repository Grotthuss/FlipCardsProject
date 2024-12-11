import React from 'react';
import './App.css';
import FlipCard from './FlipCard';
import CardSetSelection from './CardSetSelection';
import QuizFlipcard from './QuizFlipcard';
import DeleteSets from "./DeleteSet";
import DeleteCards from './DeleteCard';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from "./Login";
import Signup from "./Signup";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/signup" element={<Signup />} />
                <Route path="/sets/:user_id" element={<CardSetSelection />} />
                <Route path="/card-set/:user_id/:id" element={<FlipCard />} />
                <Route path="/quizcard/:user_id/:setId" element={<QuizFlipcard />} />
                <Route path="/delete-sets/:user_id" element={<DeleteSets />} />
                <Route path="/delete-cards/:user_id/:id" element={<DeleteCards />} />
            </Routes>
        </Router>
    );
}
/*
function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<CardSetSelection />} />
                <Route path="/card-set/:id" element={<FlipCard />} />
                <Route path="/quizcard/:userId/:setId" element={<QuizFlipcard />} />
                <Route path="/delete-sets" element={<DeleteSets />} />
                <Route path="/delete-cards/:id" element={<DeleteCards />} />
            </Routes>
        </Router>
    );
}
*/
export default App;