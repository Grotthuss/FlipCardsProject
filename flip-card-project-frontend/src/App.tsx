import React from 'react';
import './App.css';
import FlipCard from './FlipCard';
import CardSetSelection from './CardSetSelection';
import QuizFlipcard from './QuizFlipcard';
import DeleteSets from "./DeleteSet";
import DeleteCards from './DeleteCard';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

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

export default App;